using Hookr.Telegram.Operations.Base;
using Hookr.Telegram.Repository.Context;
using Hookr.Telegram.Repository.Context.Entities.Base;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;

namespace Hookr.Telegram.Operations.Commands.Administration
{
    public abstract class AdministrationCommandBase<TEntity> : CommandWithResponse where TEntity : Entity
    {
        protected abstract DbSet<TEntity> EntityTableSelector(HookrContext context);

        protected AdministrationCommandBase(IExtendedTelegramBotClient telegramBotClient,
            ITranslationsResolver translationsResolver) 
            : base(telegramBotClient,
                translationsResolver)
        {
        }
    }
    
    public abstract class AdministrationCommandBase<TEntity, TResult> : CommandWithResponse<TResult> where TEntity : Entity
    {
        protected abstract DbSet<TEntity> EntityTableSelector(HookrContext context);

        protected AdministrationCommandBase(IExtendedTelegramBotClient telegramBotClient,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                translationsResolver)
        {
        }
    }
}