using System;
using Hookr.Core.Utilities.Caches.Leveled;
using Hookr.Core.Utilities.Caching;
using Hookr.Core.Utilities.Providers;

namespace Hookr.Telegram.Utilities.Telegram.Caches.UserTemporaryStatus
{
    public class UserTemporaryStatusCache : UserLevelCache<UserTemporaryStatus>
    {
        private const int TimeoutMinutes = 3;
        public UserTemporaryStatusCache(ITelegramUserIdProvider telegramUserIdProvider) : base(telegramUserIdProvider)
        {
        }

        protected override CachingOptions MemoryCacheOptions { get; } =
            CachingOptions.Enabled(TimeSpan.FromMinutes(TimeoutMinutes));
        protected override CachingOptions DistributedCacheOptions { get; } = CachingOptions.Disabled;

        protected override string CacheKey => "temporaryStatus";
    }
}