using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context;
using Hookr.Core.Repository.Context.Entities.Translations;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;

namespace Hookr.Core.Repository
{
    public interface IHookrRepository
    {
        HookrContext Context { get; }
        Task<TResult> ReadAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor);
        Task WriteAsync(Func<HookrContext, CancellationToken, Task> functor);

        Task<TResult> WriteAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor);

        Task<int> SaveChangesAsync();
    }
}