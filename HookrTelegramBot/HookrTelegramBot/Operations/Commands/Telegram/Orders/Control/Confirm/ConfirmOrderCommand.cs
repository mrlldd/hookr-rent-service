using System.Threading.Tasks;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Confirm
{
    public class ConfirmOrderCommand : OrderCommandBase, IConfirmOrderCommand
    {
        public ConfirmOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository)
        {
        }

        protected override async Task<Order> ProcessAsync(Order order)
        {
            //todo handle already confirmed orders
            order.State = OrderStates.Confirmed;
            //todo notify service users about new order;
            await HookrRepository.Context.SaveChangesAsync();
            return order;
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Order response)
            => client.SendTextMessageAsync($"Order {response.Id} has been confirmed.");
    }
}