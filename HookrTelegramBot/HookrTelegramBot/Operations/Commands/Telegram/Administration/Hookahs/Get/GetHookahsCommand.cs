using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Caches;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Get
{
    public class GetHookahsCommand : GetCommandBase<Hookah>, IGetHookahsCommand
    {
        public GetHookahsCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserTemporaryStatusCache userTemporaryStatusCache,
            IUserContextProvider userContextProvider)
            : base(telegramBotClient,
                hookrRepository,
                userTemporaryStatusCache,
                userContextProvider)
        {
        }

        protected override DbSet<Hookah> EntityTableSelector(HookrContext context)
            => context.Hookahs;

        protected override UserTemporaryStatus NextUserState => UserTemporaryStatus.ChoosingHookah;
        
        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Hookah[] response)
            => client
                .SendTextMessageAsync(response.Any()
                    ? response
                        .Select((x, index) => $"/{index + 1} {x.Name} - {x.Price}")
                        .Aggregate((prev, next) => prev + "\n" + next)
                    : "There is no hookahs at the moment :(");

    }
}