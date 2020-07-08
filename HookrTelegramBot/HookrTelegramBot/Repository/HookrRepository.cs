using System;
using System.Threading;
using System.Threading.Tasks;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Utilities.Resiliency;

namespace HookrTelegramBot.Repository
{
    public class HookrRepository : IHookrRepository
    {
        private readonly IPolicySet policySet;
        public HookrContext Context { get; }
        public HookrRepository(HookrContext context, IPolicySet policySet)
        {
            this.policySet = policySet;
            Context = context;
        }

        public Task<TResult> ReadAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor)
            => policySet.ReadPolicy.ExecuteAsync(() => functor(Context, default));

        public Task<TResult> WriteAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor)
            => policySet.WritePolicy.ExecuteAsync(() => functor(Context, default));

    }
}