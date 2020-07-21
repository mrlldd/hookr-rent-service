using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Caches;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration
{
    public abstract class GetSingleCommandBase<TEntity> : AdministrationCommandBase<TEntity, Identified<TEntity>> where TEntity : Entity
    {
        protected readonly IUserContextProvider UserContextProvider;
        private readonly IHookrRepository hookrRepository;

        protected GetSingleCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository) : base(telegramBotClient)
        {
            UserContextProvider = userContextProvider;
            this.hookrRepository = hookrRepository;
        }

        protected override Task<Identified<TEntity>> ProcessAsync() =>
            hookrRepository
                .ReadAsync((context, token) => EntityTableSelector(context)
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


        protected abstract int ExtractIndex(string command); 
    }
}