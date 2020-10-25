using System;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Utilities.Extensions;
using Hookr.Web.Backend.Operations.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Web.Backend.Operations
{
    public class Dispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task DispatchCommandAsync<TCommand>(TCommand command, CancellationToken token = default)
            => FindAndPopulate<ICommandHandler<TCommand>>(token)
                .ExecuteCommandAsync(command);

        public Task<TResult> DispatchQueryAsync<TQuery, TResult>(TQuery query, CancellationToken token = default)
            => FindAndPopulate<IQueryHandler<TQuery, TResult>>(token)
                .ExecuteQueryAsync(query);

        private T FindAndPopulate<T>(CancellationToken token) where T : IHandler
            => serviceProvider
                .GetRequiredService<T>()
                .SideEffect(x => x.Populate(token));
    }
}