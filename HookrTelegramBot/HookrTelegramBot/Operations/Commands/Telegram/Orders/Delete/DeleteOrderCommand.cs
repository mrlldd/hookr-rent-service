using System;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders.Delete
{
    public class DeleteOrderCommand : CommandWithResponse, IDeleteOrderCommand
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IHookrRepository hookrRepository;
        private const string Space = " ";

        public DeleteOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository) : base(telegramBotClient)
        {
            this.userContextProvider = userContextProvider;
            this.hookrRepository = hookrRepository;
        }

        protected override async Task ProcessAsync()
        {
            var orderId = ExtractArguments(userContextProvider.Update.Content);
            var order = await hookrRepository.ReadAsync((context, token) => context.Orders
                .FirstOrDefaultAsync(x => x.Id == orderId, token));
            var user = userContextProvider.DatabaseUser;
            if (order == null || user.State == TelegramUserStates.Default && order.CreatedById != user.Id)
            {
                throw new InvalidOperationException("Seems like you have no access :(");
            }
            hookrRepository.Context.Orders.Remove(order);
            await hookrRepository.Context.SaveChangesAsync();
        }

        private static int ExtractArguments(string command)
        {
            var subs = command.Split(Space);
            if (subs.Length != 2)
            {
                throw new InvalidOperationException("Wrong arguments have been passed in.");
            }

            return int.TryParse(subs.Last(), out var result)
                ? result
                : throw new InvalidOperationException("Wrong arguments have been passed in.");
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => client.SendTextMessageAsync("Successfully deleted order.");
    }
}