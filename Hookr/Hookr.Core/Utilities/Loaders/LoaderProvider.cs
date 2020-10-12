using System;
using Hookr.Core.Utilities.Caching;
using Hookr.Core.Utilities.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

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

        public CachingLoader<TArgs, TResult> Get<TArgs, TResult>()
            where TResult : class
            => InternalGet<CachingLoader<TArgs, TResult>, TResult>();
    }
}