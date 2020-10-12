using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context.Entities;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Core.Utilities.Extensions;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Extensions;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Notifiers;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Operations.Commands.Orders.Control.Service.Review.Finish
{
    public class FinishOrderCommand : ReviewOrderCommandBase, IFinishOrderCommand
    {
        public FinishOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            ITelegramHookrRepository hookrRepository,
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
                .SendTextMessageAsync(await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.OrderHasBeenFinished, response.Id));

        protected override OrderStates AllowedState => OrderStates.Processing;
        protected override OrderStates NextOrderState => OrderStates.Finished;
        protected override async Task<IEnumerable<Message>> NotifyAsync(Order order, TelegramUser user)
        {
            var (notificationTranslated, orderHasBeenFinishedByTranslated, yourOrderHasBeenFinishedTranslated) =
                await TranslationsResolver.ResolveAsync(
                    (TelegramTranslationKeys.Notification, Array.Empty<object>()),
                    (TelegramTranslationKeys.OrderHasBeenFinishedBy, new object[]
                    {
                        order.Id,
                        user.Username,
                        UserContextProvider.DatabaseUser.Username
                    }),
                    (TelegramTranslationKeys.YourOrderHasBeenFinished, new object[]
                    {
                        order.Id
                    }));
            // todo translations
            var result = await new Func<Task<IEnumerable<Message>>>[]
                {
                    () => TelegramUsersNotifier
                        .SendAsync(
                            (client, telegramUser) => client
                                .SendTextMessageAsync(telegramUser.Id,
                                    $"[{notificationTranslated}] {orderHasBeenFinishedByTranslated}"),
                            TelegramUserStates.Service,
                            TelegramUserStates.Dev),
                    () => TelegramUsersNotifier
                        .SendAsync((client, telegramUser) => client
                                .SendTextMessageAsync(telegramUser.Id,
                                    yourOrderHasBeenFinishedTranslated),
                            user)
                }
                .ExecuteMultipleAsync();
            return result
                .Linear();
        }
    }
}