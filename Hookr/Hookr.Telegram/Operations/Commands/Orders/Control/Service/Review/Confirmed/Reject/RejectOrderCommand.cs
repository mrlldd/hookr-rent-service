using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Core.Utilities.Extensions;
using Hookr.Telegram.Utilities.Extensions;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Notifiers;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Operations.Commands.Orders.Control.Service.Review.Confirmed.Reject
{
    public class RejectOrderCommand : ReviewConfirmedOrderCommandBase, IRejectOrderCommand
    {
        public RejectOrderCommand(IExtendedTelegramBotClient telegramBotClient,
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
                .SendTextMessageAsync(await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.OrderHasBeenRejected, response.Id));

        protected override OrderStates NextOrderState => OrderStates.Rejected;
        protected override async Task<IEnumerable<Message>> NotifyAsync(Order order, TelegramUser user)
        {
            var(notificationTranslated, orderHasBeenRejectedByTranslated, yourOrderHasBeenRejectedTranslated) = await TranslationsResolver.ResolveAsync(
                (TelegramTranslationKeys.Notification, Array.Empty<object>()),
                (TelegramTranslationKeys.OrderHasBeenRejectedBy, new object[]
                {
                    order.Id,
                    user.Username,
                    UserContextProvider.DatabaseUser.Username
                }),
                (TelegramTranslationKeys.YourOrderHasBeenRejected, new object[]
                {
                    order.Id
                }));
            var result = await new Func<Task<IEnumerable<Message>>>[]
                {
                    () => TelegramUsersNotifier
                        .SendAsync(
                            (client, telegramUser) => client
                                .SendTextMessageAsync(telegramUser.Id,
                                    $"[{notificationTranslated}] {orderHasBeenRejectedByTranslated}"),
                            TelegramUserStates.Service,
                            TelegramUserStates.Dev),
                    () => TelegramUsersNotifier
                        .SendAsync((client, telegramUser) => client
                                .SendTextMessageAsync(telegramUser.Id,
                                    yourOrderHasBeenRejectedTranslated),
                            user)
                }
                .ExecuteMultipleAsync();
            return result
                .Linear();
        }
    }
}