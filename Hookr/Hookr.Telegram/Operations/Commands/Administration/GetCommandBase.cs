using System.Threading.Tasks;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Repository.Context.Entities.Base;
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
        private readonly IHookrRepository hookrRepository;
        private readonly IUserTemporaryStatusCache userTemporaryStatusCache;
        private readonly IUserContextProvider userContextProvider;

        protected GetCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserTemporaryStatusCache userTemporaryStatusCache,
            IUserContextProvider userContextProvider,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                translationsResolver)
        {
            this.hookrRepository = hookrRepository;
            this.userTemporaryStatusCache = userTemporaryStatusCache;
            this.userContextProvider = userContextProvider;
        }

        protected override Task<TEntity[]> ProcessAsync()
            => hookrRepository
                .ReadAsync((context, token)
                    => EntityTableSelector(context).ToArrayAsync(token))
                .ContinueWith(x =>
                {
                    if (x.IsCompletedSuccessfully)
                    {
                        var update = userContextProvider.Update;
                        userTemporaryStatusCache.Set(
                            update.Type == UpdateType.CallbackQuery
                                ? update.CallbackQuery.From.Id
                                : update.RealMessage.From.Id,
                            NextUserState);
                    }

                    return x.Result;
                });

        protected abstract UserTemporaryStatus NextUserState { get; }
    }
}