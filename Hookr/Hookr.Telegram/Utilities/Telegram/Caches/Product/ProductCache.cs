using System;
using Hookr.Core.Utilities.Caching;

namespace Hookr.Telegram.Utilities.Telegram.Caches.Product
{
    public class ProductCache : Cache<int>
    {
        private const int TimeoutMinutes = 1;
        protected override CachingOptions MemoryCacheOptions { get; } =
            new CachingOptions(true, TimeSpan.FromMinutes(TimeoutMinutes));
        protected override CachingOptions DistributedCacheOptions { get; } = 
            new CachingOptions(false, TimeSpan.Zero);

        protected override string CacheKey => "product";
    }
}