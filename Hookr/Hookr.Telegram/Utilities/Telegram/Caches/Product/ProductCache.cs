using System;
using Hookr.Core.Utilities.Caches.Leveled;
using Hookr.Core.Utilities.Caching;
using Hookr.Core.Utilities.Providers;

namespace Hookr.Telegram.Utilities.Telegram.Caches.Product
{
    public class ProductCache : UserLevelCache<Product>
    {
        public ProductCache(ITelegramUserIdProvider telegramUserIdProvider) : base(telegramUserIdProvider)
        {
        }
        private const int TimeoutMinutes = 1;
        protected override CachingOptions MemoryCacheOptions { get; } =
            new CachingOptions(true, TimeSpan.FromMinutes(TimeoutMinutes));
        protected override CachingOptions DistributedCacheOptions { get; } = CachingOptions.False;

        protected override string CacheKey => "product";

    }
}