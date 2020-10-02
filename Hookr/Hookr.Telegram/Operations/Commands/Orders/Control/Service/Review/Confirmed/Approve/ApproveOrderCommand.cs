using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Repository.Context.Entities;
using Hookr.Telegram.Repository.Context.Entities.Base;
using Hookr.Telegram.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Utilities.Extensions;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Notifiers;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Operations.Commands.Orders.Control.Service.Review.Confirmed.Approve
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
                .SendTextMessageAsync(await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.OrderHasBeenApproved, response.Id));

        protected sealed override OrderStates NextOrderState => OrderStates.Approved;

        protected override async Task<IEnumerable<Message>> NotifyAsync(Order order, TelegramUser user)
        {
            var (notificationTranslated, orderhasBeenApprovedByTranslated, yourOrderHasBeenApprovedTranslated) =
                await TranslationsResolver.ResolveAsync(
                    (TelegramTranslationKeys.Notification, Array.Empty<object>()),
                    (TelegramTranslationKeys.OrderHasBeenApprovedBy, new object[]
                    {
                        order.Id,
                        user.Username,
                        UserContextProvider.DatabaseUser.Username
                    }),
                    (TelegramTranslationKeys.YourOrderHasBeenApproved, new object[]
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