using System;
using System.Threading.Tasks;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Notifiers;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Confirm
{
    public class ConfirmOrderCommand : OrderCommandBase, IConfirmOrderCommand
    {
        private readonly ITelegramUsersNotifier telegramUsersNotifier;

        public ConfirmOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
            ITelegramUsersNotifier telegramUsersNotifier)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository)
        {
            this.telegramUsersNotifier = telegramUsersNotifier;
        }

        protected override async Task<Order> ProcessAsync(Order order)
        {
            if (order.State != OrderStates.Constructing)
            {
                throw new InvalidOperationException("Order is already confirmed.");
            }

            order.State = OrderStates.Confirmed;
            //todo notify service users about new order;
            await HookrRepository.Context.SaveChangesAsync();
            await telegramUsersNotifier.SendAsync((client, user) =>
                    client.SendTextMessageAsync(user.Id, "New order:\n" +
                                                         $"Id: {order.Id}"),
                TelegramUserStates.Service,
                TelegramUserStates.Dev);
            return order;
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Order response)
            => client.SendTextMessageAsync($"Order {response.Id} has been confirmed.");
    }
}