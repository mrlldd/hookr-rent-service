using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hookr.Core.Repository
{
    public interface IRepository<out TContext> where TContext : DbContext
    {
        TContext Context { get; }
        Task<TResult> ReadAsync<TResult>(Func<TContext, CancellationToken, Task<TResult>> functor, CancellationToken cancellationToken = default);
        Task WriteAsync(Func<TContext, CancellationToken, Task> functor, CancellationToken cancellationToken = default);

        Task<TResult> WriteAsync<TResult>(Func<TContext, CancellationToken, Task<TResult>> functor, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}