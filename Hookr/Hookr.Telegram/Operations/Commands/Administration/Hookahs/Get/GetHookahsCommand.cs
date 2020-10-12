using System.Linq;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context;
using Hookr.Core.Repository.Context.Entities.Products;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Core.Utilities.Caches;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Extensions;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Caches.UserTemporaryStatus;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Operations.Commands.Administration.Hookahs.Get
{
    public class GetHookahsCommand : GetCommandBase<Hookah>, IGetHookahsCommand
    {

        public GetHookahsCommand(IExtendedTelegramBotClient telegramBotClient,
            ITelegramHookrRepository hookrRepository,
            ICacheProvider cacheProvider,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                hookrRepository,
                cacheProvider,
                translationsResolver)
        {
        }

        protected override DbSet<Hookah> EntityTableSelector(HookrContext context)
            => context.Hookahs;

        protected override UserTemporaryStatus NextUserState => UserTemporaryStatus.ChoosingHookah;
        
        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Hookah[] response)
            => await client
                .SendTextMessageAsync(response.Any()
                    ? response
                        .AggregateListString("/{0} {1} - {2}",
                            x => x.Name,
                            x => x.Price)
                    : await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.NoHookahs));

    }
}