using System.Threading.Tasks;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Utilities.Caches;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Caches.UserTemporaryStatus;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;

namespace Hookr.Telegram.Operations.Commands.Administration
{
    public abstract class GetCommandBase<TEntity> : AdministrationCommandBase<TEntity, TEntity[]>
        where TEntity : Entity
    {
        private readonly ITelegramHookrRepository hookrRepository;
        private readonly ICacheProvider cacheProvider;

        protected GetCommandBase(IExtendedTelegramBotClient telegramBotClient,
            ITelegramHookrRepository hookrRepository,
            ICacheProvider cacheProvider,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                translationsResolver)
        {
            this.hookrRepository = hookrRepository;
            this.cacheProvider = cacheProvider;
        }

        protected override async Task<TEntity[]> ProcessAsync()
        {
            var entities = await hookrRepository
                .ReadAsync((context, token)
                    => EntityTableSelector(context).ToArrayAsync(token));
            await cacheProvider
                .UserLevel<UserTemporaryStatus>()
                .SetAsync(NextUserState);
            return entities;
        }

        protected abstract UserTemporaryStatus NextUserState { get; }
    }
}