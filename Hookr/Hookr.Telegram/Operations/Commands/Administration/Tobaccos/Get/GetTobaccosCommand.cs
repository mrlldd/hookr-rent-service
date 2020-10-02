using System.Linq;
using System.Threading.Tasks;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Repository.Context;
using Hookr.Telegram.Repository.Context.Entities.Products;
using Hookr.Telegram.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Utilities.Extensions;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Caches.UserTemporaryStatus;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Operations.Commands.Administration.Tobaccos.Get
{
    public class GetTobaccosCommand : GetCommandBase<Tobacco>, IGetTobaccosCommand
    {

        public GetTobaccosCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserTemporaryStatusCache userTemporaryStatusCache,
            IUserContextProvider userContextProvider,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                hookrRepository,
                userTemporaryStatusCache,
                userContextProvider,
                translationsResolver)
        {
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Tobacco[] response)
            => await client
                .SendTextMessageAsync(response.Any()
                    ? response
                        .AggregateListString("/{0} {1} - {2}",
                            x => x.Name,
                            x => x.Price)
                    : await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.NoTobaccos));

        protected override DbSet<Tobacco> EntityTableSelector(HookrContext context)
            => context.Tobaccos;

        protected override UserTemporaryStatus NextUserState => UserTemporaryStatus.ChoosingTobacco;

    }
}