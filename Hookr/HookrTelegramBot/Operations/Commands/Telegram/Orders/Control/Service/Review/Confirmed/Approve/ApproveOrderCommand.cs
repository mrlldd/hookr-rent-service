using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Notifiers;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Service.Review.Confirmed.Approve
{
    public class ApproveOrderCommand : ReviewConfirmedOrderCommandBase, IApproveOrderCommand
    {
        public ApproveOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
            ITranslationsResolver translationsResolver,
            ITelegramUsersNotifier telegramUsersNotifier)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository,
                translationsResolver,
                telegramUsersNotifier)
        {
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Order response)
            => await client
                .SendTextMessageAsync(await TranslationsResolver.ResolveAsync(TranslationKeys.OrderHasBeenApproved, response.Id));

        protected sealed override OrderStates NextOrderState => OrderStates.Approved;

        protected override async Task<IEnumerable<Message>> NotifyAsync(Order order, TelegramUser user)
        {
            var (notificationTranslated, orderhasBeenApprovedByTranslated, yourOrderHasBeenApprovedTranslated) =
                await TranslationsResolver.ResolveAsync(
                    (TranslationKeys.Notification, Array.Empty<object>()),
                    (TranslationKeys.OrderHasBeenApprovedBy, new object[]
                    {
                        order.Id,
                        user.Username,
                        UserContextProvider.DatabaseUser.Username
                    }),
                    (TranslationKeys.YourOrderHasBeenApproved, new object[]
                    {
                        order.Id
                    }));
            var result = await new Func<Task<IEnumerable<Message>>>[]
                {
                    () => TelegramUsersNotifier
                        .SendAsync(
                            (client, telegramUser) => client
                                .SendTextMessageAsync(telegramUser.Id,
                                    $"[{notificationTranslated}] {orderhasBeenApprovedByTranslated}"),
                            TelegramUserStates.Service,
                            TelegramUserStates.Dev),
                    () => TelegramUsersNotifier
                        .SendAsync((client, telegramUser) => client
                                .SendTextMessageAsync(telegramUser.Id,
                                    yourOrderHasBeenApprovedTranslated),
                            user)
                }
                .ExecuteMultipleAsync();
            return result
                .Linear();
        }
    }
}