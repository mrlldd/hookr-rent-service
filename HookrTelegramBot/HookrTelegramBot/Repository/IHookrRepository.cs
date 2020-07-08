using System;
using System.Threading;
using System.Threading.Tasks;
using HookrTelegramBot.Repository.Context;

namespace HookrTelegramBot.Repository
{
    public interface IHookrRepository
    {
        HookrContext Context { get; }
        Task<TResult> ReadAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor);
        Task WriteAsync(Func<HookrContext, CancellationToken, Task> functor);
    }
}