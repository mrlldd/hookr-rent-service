using System;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Utilities.Extensions;
using Hookr.Core.Utilities.Providers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hookr.Core.Utilities.Loaders
{
    public class LoaderDispatcher
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        public LoaderDispatcher(
            IServiceProvider serviceProvider,
            IMemoryCache memoryCache,
            IDistributedCache distributedCache)
        {
            this.serviceProvider = serviceProvider;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }

        public Task<TResult> GetOrLoadAsync<TArgs, TResult>(TArgs args,
            bool omitCache = false,
            CancellationToken token = default) where TResult : class
            => serviceProvider
                .GetRequiredService<CachingLoader<TArgs, TResult>>()
                .Unite(serviceProvider.GetRequiredService<ILogger<CachingLoader<TArgs, TResult>>>())
                .SideEffect(x => x.First
                    .Populate(memoryCache, distributedCache, x.Second)
                )
                .First.GetOrLoadAsync(args, omitCache, token);
    }
}