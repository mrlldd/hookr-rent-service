using System;
using Hookr.Core.Utilities.Caches.Leveled;
using Hookr.Core.Utilities.Caching;
using Hookr.Core.Utilities.Providers;

namespace Hookr.Telegram.Utilities.Telegram.Caches.CurrentOrder
{
    public class CurrentOrderCache : UserLevelCache<CurrentOrder>
    {
        private const int TimeoutMinutes = 33;
        public CurrentOrderCache(ITelegramUserIdProvider telegramUserIdProvider) : base(telegramUserIdProvider)
        {
        }
        

        protected override CachingOptions MemoryCacheOptions { get; } = 
            CachingOptions.Enabled(TimeSpan.FromMinutes(TimeoutMinutes));
        protected override CachingOptions DistributedCacheOptions { get; } = CachingOptions.Disabled;

        protected override string CacheKey => "currentOrder";
    }
}