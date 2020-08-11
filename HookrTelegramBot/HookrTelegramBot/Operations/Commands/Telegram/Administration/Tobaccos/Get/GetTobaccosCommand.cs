using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Caches;
using HookrTelegramBot.Utilities.Telegram.Caches.UserTemporaryStatus;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Get
{
    public class GetTobaccosCommand : GetCommandBase<Tobacco>, IGetTobaccosCommand
    {
        private readonly ITranslationsResolver translationsResolver;

        public GetTobaccosCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserTemporaryStatusCache userTemporaryStatusCache,
            IUserContextProvider userContextProvider,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                hookrRepository,
                userTemporaryStatusCache,
                userContextProvider)
        {
            this.translationsResolver = translationsResolver;
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Tobacco[] response)
            => await client
                .SendTextMessageAsync(response.Any()
                    ? response
                        .Select((x, index) => $"/{index + 1} {x.Name} - {x.Price}")
                        .Aggregate((prev, next) => prev + "\n" + next)
                    : await translationsResolver.ResolveAsync(TranslationKeys.NoTobaccos));

        protected override DbSet<Tobacco> EntityTableSelector(HookrContext context)
            => context.Tobaccos;

        protected override UserTemporaryStatus NextUserState => UserTemporaryStatus.ChoosingTobacco;

    }
}