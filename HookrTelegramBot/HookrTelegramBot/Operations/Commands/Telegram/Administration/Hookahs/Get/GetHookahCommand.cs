using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Get
{
    public class GetHookahCommand : CommandWithResponse<Hookah>, IGetHookahCommand
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IHookrRepository hookrRepository;

        public GetHookahCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository) : base(telegramBotClient)
        {
            this.userContextProvider = userContextProvider;
            this.hookrRepository = hookrRepository;
        }

        protected override async Task<Hookah> ProcessAsync()
        {
            var id = ExtractArguments(userContextProvider.Update.RealMessage.Text);
            var hookahs = await hookrRepository.ReadAsync((context, token)
                => context.Hookahs.ToArrayAsync(token));
            return hookahs.ElementAt(id - 1);
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Hookah response)
            => client
                .SendTextMessageAsync($"Here is your hookah {response.Name} - {response.Price}");

        private static int ExtractArguments(string messageText)
            => int
                .Parse(messageText
                    .Trim()
                    .Substring(1));
    }
}