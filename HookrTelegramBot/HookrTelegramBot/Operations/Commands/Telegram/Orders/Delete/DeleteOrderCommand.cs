using System;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram.Exceptions;
using HookrTelegramBot.Models.Telegram.Exceptions.Base;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders.Delete
{
    public class DeleteOrderCommand : OrderCommandBase, IDeleteOrderCommand
    {
        public DeleteOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository)
        {
        }

        protected override async Task<Order> ProcessAsync(Order order)
        {
            HookrRepository.Context.Orders.Remove(order);
            await HookrRepository.Context.SaveChangesAsync();
            return order;
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Order response)
            => client.SendTextMessageAsync($"Successfully deleted order {response.Id}.");

        protected override (bool, string) HandleCustomException(Exception exception)
            => exception is OrderAlreadyDeletedException
                ? (true, "Order not exist or has been already deleted.")
                : base.HandleCustomException(exception);
    }
}