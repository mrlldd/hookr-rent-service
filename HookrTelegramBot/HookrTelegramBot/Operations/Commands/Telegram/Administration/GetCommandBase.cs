using System.Collections.Generic;
using System.Linq;
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
using Telegram.Bot.Types.Enums;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration
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
            IUserContextProvider userContextProvider) : base(telegramBotClient)
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