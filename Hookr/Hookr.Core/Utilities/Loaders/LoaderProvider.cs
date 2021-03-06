using System;
using Hookr.Core.Utilities.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Hookr.Core.Utilities.Loaders
{
    internal sealed class LoaderProvider : CachingProvider, ILoaderProvider
    {
        public LoaderProvider(
            IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            IServiceProvider serviceProvider) 
            : base(memoryCache, distributedCache, serviceProvider)
        {
        }

        public ICachingLoader<TArgs, TResult> Get<TArgs, TResult>()
            where TResult : class
            => InternalGet<ICachingLoader<TArgs, TResult>, TResult>();
    }
}