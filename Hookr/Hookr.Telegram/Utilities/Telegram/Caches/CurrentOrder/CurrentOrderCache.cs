using System;
using Hookr.Core.Utilities.Caching;

namespace Hookr.Telegram.Utilities.Telegram.Caches.CurrentOrder
{
    public class CurrentOrderCache : Cache<int?>, ICurrentOrderCache
    {
        private const int TimeoutMinutes = 33;

        protected override CachingOptions MemoryCacheOptions { get; } =
            new CachingOptions(true, TimeSpan.FromMinutes(TimeoutMinutes));
        protected override CachingOptions DistributedCacheOptions { get; } =
            new CachingOptions(false, TimeSpan.Zero);

        protected override string CacheKey => "currentOrder";
    }
}