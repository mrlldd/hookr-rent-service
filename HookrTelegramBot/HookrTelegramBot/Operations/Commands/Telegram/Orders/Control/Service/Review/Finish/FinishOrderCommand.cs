using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Notifiers;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Service.Review.Finish
{
    public class FinishOrderCommand : ReviewOrderCommandBase, IFinishOrderCommand
    {
        public FinishOrderCommand(IExtendedTelegramBotClient telegramBotClient,
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

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Order response)
            => client
                .SendTextMessageAsync($"Order with id {response.Id} has been finished.");

        protected override OrderStates AllowedState => OrderStates.Processing;
        protected override OrderStates NextOrderState => OrderStates.Finished;
        protected override async Task<IEnumerable<Message>> NotifyAsync(Order order, TelegramUser user)
        {
            var result = await new Func<Task<IEnumerable<Message>>>[]
                {
                    () => TelegramUsersNotifier
                        .SendAsync(
                            (client, telegramUser) => client
                                .SendTextMessageAsync(telegramUser.Id,
                                    $"[Notification] Order with id {order.Id} by user @{user.Username} has been finished by @{UserContextProvider.DatabaseUser.Username}."),
                            TelegramUserStates.Service,
                            TelegramUserStates.Dev),
                    () => TelegramUsersNotifier
                        .SendAsync((client, telegramUser) => client
                                .SendTextMessageAsync(telegramUser.Id,
                                    $"Your order with id {order.Id} has been finished."),
                            user)
                }
                .ExecuteMultipleAsync();
            return result
                .Linear();
        }
    }
}