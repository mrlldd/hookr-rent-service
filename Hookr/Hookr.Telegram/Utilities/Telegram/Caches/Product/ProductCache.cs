using System;
using Hookr.Core.Utilities.Caching;
using Hookr.Core.Utilities.Providers;

namespace Hookr.Telegram.Utilities.Telegram.Caches.Product
{
    public class ProductCache : UserLevelCache<int>, IProductCache
    {
        public ProductCache(ITelegramUserIdProvider telegramUserIdProvider) : base(telegramUserIdProvider)
        {
        }
        private const int TimeoutMinutes = 1;
        protected override CachingOptions MemoryCacheOptions { get; } =
            new CachingOptions(true, TimeSpan.FromMinutes(TimeoutMinutes));
        protected override CachingOptions DistributedCacheOptions { get; } = 
            new CachingOptions(false, TimeSpan.Zero);

        protected override string CacheKey => "product";

    }
}