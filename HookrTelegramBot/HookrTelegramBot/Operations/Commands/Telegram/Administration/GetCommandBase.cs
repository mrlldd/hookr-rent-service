using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Caches;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration
{
    public abstract class GetCommandBase<TEntity> : AdministrationCommandBase<TEntity, TEntity[]>
        where TEntity : Entity
    {
        private readonly IHookrRepository hookrRepository;
        private readonly IUserTemporaryStatusCache userTemporaryStatusCache;

        public GetCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserTemporaryStatusCache userTemporaryStatusCache) : base(telegramBotClient)
        {
            this.hookrRepository = hookrRepository;
            this.userTemporaryStatusCache = userTemporaryStatusCache;
        }

        protected override Task<TEntity[]> ProcessAsync()
            => hookrRepository
                .ReadAsync((context, token)
                    => EntityTableSelector(context).ToArrayAsync(token))
                .ContinueWith(x =>
                {
                    if (x.IsCompletedSuccessfully)
                    {
                        userTemporaryStatusCache.Set(TelegramBotClient.WithCurrentUser.User.Id,
                            NextUserState);
                    }

                    return x.Result;
                });

        protected abstract UserTemporaryStatus NextUserState { get; }
    }
}