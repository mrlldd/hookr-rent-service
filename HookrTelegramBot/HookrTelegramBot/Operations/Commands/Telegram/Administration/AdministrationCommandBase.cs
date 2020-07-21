using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using Microsoft.EntityFrameworkCore;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration
{
    public abstract class AdministrationCommandBase<TEntity> : CommandWithResponse where TEntity : Entity
    {
        protected abstract DbSet<TEntity> EntityTableSelector(HookrContext context);

        protected AdministrationCommandBase(IExtendedTelegramBotClient telegramBotClient) : base(telegramBotClient)
        {
        }
    }
    
    public abstract class AdministrationCommandBase<TEntity, TResult> : CommandWithResponse<TResult> where TEntity : Entity
    {
        protected abstract DbSet<TEntity> EntityTableSelector(HookrContext context);

        protected AdministrationCommandBase(IExtendedTelegramBotClient telegramBotClient) : base(telegramBotClient)
        {
        }
    }
}