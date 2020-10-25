using System;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context;
using Hookr.Core.Utilities.Loaders;
using Hookr.Core.Utilities.Providers;
using Hookr.Core.Utilities.Resiliency;

namespace Hookr.Core.Repository
{
    public class HookrRepository : IHookrRepository
    {
        protected readonly IPolicySet PolicySet;
        private readonly ITelegramUserIdProvider telegramUserIdProvider;
        private readonly ILoaderProvider loaderProvider;
        public HookrContext Context { get; }

        public HookrRepository(HookrContext context,
            IPolicySet policySet,
            ITelegramUserIdProvider telegramUserIdProvider,
            ILoaderProvider loaderProvider)
        {
            PolicySet = policySet;
            this.telegramUserIdProvider = telegramUserIdProvider;
            this.loaderProvider = loaderProvider;
            Context = context;
        }

        public Task<TResult> ReadAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor,
            CancellationToken cancellationToken = default)
            => PolicySet.ReadPolicy.ExecuteAsync(() => functor(Context, cancellationToken));

        public Task WriteAsync(Func<HookrContext, CancellationToken, Task> functor,
            CancellationToken cancellationToken = default)
            => PolicySet.WritePolicy.ExecuteAsync(() => functor(Context, cancellationToken));

        public Task<TResult> WriteAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor,
            CancellationToken cancellationToken = default)
            => PolicySet.WritePolicy.ExecuteAsync(() => functor(Context, cancellationToken));

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => WriteAsync((context, token) => context.SaveChangesAsync(telegramUserIdProvider, loaderProvider, token), cancellationToken);
    }
}