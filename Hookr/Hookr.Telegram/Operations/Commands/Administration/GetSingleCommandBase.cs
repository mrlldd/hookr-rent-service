using System.Linq;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Telegram.Models.Telegram;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;

namespace Hookr.Telegram.Operations.Commands.Administration
{
    public abstract class GetSingleCommandBase<TEntity> : AdministrationCommandBase<TEntity, Identified<TEntity>>
        where TEntity : Entity
    {
        protected readonly IUserContextProvider UserContextProvider;
        private readonly IHookrRepository hookrRepository;

        protected GetSingleCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                translationsResolver)
        {
            UserContextProvider = userContextProvider;
            this.hookrRepository = hookrRepository;
        }

        protected override Task<Identified<TEntity>> ProcessAsync() =>
            hookrRepository
                .ReadAsync((context, token) => SideQuery(EntityTableSelector(context))
                    .ToArrayAsync(token))
                .ContinueWith(task =>
                {
                    var id = ExtractIndex(UserContextProvider.Update.RealMessage.Text);
                    return new Identified<TEntity>
                    {
                        Entity = task.Result[id - 1],
                        Index = id
                    };
                });

        protected virtual IQueryable<TEntity> SideQuery(IQueryable<TEntity> query) => query;


        protected abstract int ExtractIndex(string command);
    }
}