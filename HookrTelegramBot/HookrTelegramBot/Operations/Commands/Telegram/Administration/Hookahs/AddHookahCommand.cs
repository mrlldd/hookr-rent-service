using System;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs
{
    public class AddHookahCommand : CommandWithResponse, IAddHookahCommand
    {
        private const string Space = " ";
        private readonly IHookrRepository hookrRepository;
        private readonly IUserContextProvider userContextProvider;

        public AddHookahCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserContextProvider userContextProvider) : base(telegramBotClient)
        {
            this.hookrRepository = hookrRepository;
            this.userContextProvider = userContextProvider;
        }

        protected override async Task ProcessAsync()
        {
            if (userContextProvider.DatabaseUser.State < TelegramUserStates.Service)
            {
                throw new InvalidOperationException("No access rights to do that :(");
            }
            var (name, price) = ExtractArguments(userContextProvider.Update.RealMessage.Text);
            await hookrRepository.Context.Hookahs.AddAsync(new Hookah
            {
                Name = name,
                Price = price
            });
            await hookrRepository.Context.SaveChangesAsync();
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => client.SendTextMessageAsync("Successfully added new hookah");

        private static (string name, int price) ExtractArguments(string command)
        {
            var subs = command.Split(Space);
            if (subs.Length != 3 || !int.TryParse(subs[2], out var price))
            {
                throw new InvalidOperationException("Wrongs arguments passed in.");
            }

            return (subs[1], price);
        }
    }
}