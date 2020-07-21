using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Caches;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Get
{
    public class GetTobaccosCommand : GetCommandBase<Tobacco>, IGetTobaccosCommand
    {
        public GetTobaccosCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserTemporaryStatusCache userTemporaryStatusCache)
            : base(telegramBotClient,
                hookrRepository,
                userTemporaryStatusCache)
        {
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Tobacco[] response)
            => client
                .SendTextMessageAsync(response.Any()
                    ? response
                        .Select((x, index) => $"/{index + 1} {x.Name} - {x.Price}")
                        .Aggregate((prev, next) => prev + "\n" + next)
                    : "There is no tobaccos at the moment :(");

        protected override DbSet<Tobacco> EntityTableSelector(HookrContext context)
            => context.Tobaccos;

        protected override UserTemporaryStatus NextUserState => UserTemporaryStatus.ChoosingTobacco;
    }
}