using System;
using System.Threading.Tasks;
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

        public Task<TResult> DispatchQueryAsync<TQuery, TResult>(TQuery query) 
            => serviceProvider
                .GetService<IQueryHandler<TQuery, TResult>>()
                .ExecuteQueryAsync(query);
    }
}