using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Utilities.Extensions;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Caches.UserTemporaryStatus;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Enums;

namespace Hookr.Telegram.Operations.Commands.Administration
{
    public abstract class GetCommandBase<TEntity> : AdministrationCommandBase<TEntity, TEntity[]>
        where TEntity : Entity
    {
        private readonly ITelegramHookrRepository hookrRepository;
        private readonly IUserTemporaryStatusCache userTemporaryStatusCache;

        protected GetCommandBase(IExtendedTelegramBotClient telegramBotClient,
            ITelegramHookrRepository hookrRepository,
            IUserTemporaryStatusCache userTemporaryStatusCache,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                translationsResolver)
        {
            this.hookrRepository = hookrRepository;
            this.userTemporaryStatusCache = userTemporaryStatusCache;
        }

        protected override async Task<TEntity[]> ProcessAsync()
        {
            var entities = await hookrRepository
                .ReadAsync((context, token)
                    => EntityTableSelector(context).ToArrayAsync(token));
            await userTemporaryStatusCache.SetAsync(NextUserState);
            return entities;
        }

        protected abstract UserTemporaryStatus NextUserState { get; }
    }
}