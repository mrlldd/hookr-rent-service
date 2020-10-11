using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context;
using Hookr.Core.Repository.Context.Entities.Translations;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Core.Utilities.Extensions;
using Hookr.Core.Utilities.Loaders;
using Hookr.Core.Utilities.Providers;
using Hookr.Core.Utilities.Resiliency;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace Hookr.Core.Repository
{
    public class HookrRepository : IHookrRepository
    {
        protected readonly IPolicySet PolicySet;
        private readonly ITelegramUserIdProvider telegramUserIdProvider;
        private readonly LoaderDispatcher loaderDispatcher;
        public HookrContext Context { get; }

        public HookrRepository(HookrContext context,
            IPolicySet policySet,
            ITelegramUserIdProvider telegramUserIdProvider,
            LoaderDispatcher loaderDispatcher)
        {
            this.PolicySet = policySet;
            this.telegramUserIdProvider = telegramUserIdProvider;
            this.loaderDispatcher = loaderDispatcher;
            Context = context;
        }

        public Task<TResult> ReadAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor)
            => PolicySet.ReadPolicy.ExecuteAsync(() => functor(Context, default));

        public Task WriteAsync(Func<HookrContext, CancellationToken, Task> functor)
            => PolicySet.WritePolicy.ExecuteAsync(() => functor(Context, default));

        public Task<TResult> WriteAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor)
            => PolicySet.WritePolicy.ExecuteAsync(() => functor(Context, default));

        public Task<int> SaveChangesAsync()
            => WriteAsync((context, token) => context.SaveChangesAsync(telegramUserIdProvider, loaderDispatcher, token));


    }
}