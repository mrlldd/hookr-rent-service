using System;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders
{
    public class AddToOrderCommand : CommandWithResponse, IAddToOrderCommand
    {
        private const string Space = " ";
        private readonly IHookrRepository hookrRepository;
        private readonly IUserContextProvider userContextProvider;

        public AddToOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserContextProvider userContextProvider) : base(telegramBotClient)
        {
            this.hookrRepository = hookrRepository;
            this.userContextProvider = userContextProvider;
        }

        protected override Task ProcessAsync()
        {
            throw new System.NotImplementedException();
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
        {
            throw new System.NotImplementedException();
        }

        private (int OrderId, string ProductName, int ProductIndex) ExtractArguments(string command)
        {
            var subs = command
                .Split(Space)
                .Skip(1)
                .ToArray();
            if (subs.Length != 3)
            {
                throw new InvalidOperationException("Wrong arguments have been passed in.");
            }

            var productName = subs[1];
            if (!(int.TryParse(subs[0], out var orderId)
                  && int.TryParse(subs[3], out var tobaccoIndex)
                  && (
                      productName.Equals(nameof(Tobacco))
                      || productName.Equals(nameof(Hookah)))
                ))
            {
                throw new InvalidOperationException("Wrong arguments have been passed in.");
            }

            return (orderId, productName, tobaccoIndex);
        }
    }
}