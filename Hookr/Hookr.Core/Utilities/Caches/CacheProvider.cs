using System;
using Hookr.Core.Utilities.Caches.Leveled;
using Hookr.Core.Utilities.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Hookr.Core.Utilities.Caches
{
    internal sealed class CacheProvider : CachingProvider, ICacheProvider
    {
        public CacheProvider(IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            IServiceProvider serviceProvider)
            : base(memoryCache,
                distributedCache,
                serviceProvider)
        {
        }

        public ICache<T> UserLevel<T>()
            => InternalGet<IUserLevelCache<T>, T>();

        public ICache<T> ApplicationLevel<T>()
            => InternalGet<IApplicationLevelCache<T>, T>();
    }
}