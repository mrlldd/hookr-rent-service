using System;
using Hookr.Core.Utilities.Caches.Leveled;
using Hookr.Core.Utilities.Caching;
using Hookr.Core.Utilities.Providers;

namespace Hookr.Web.Backend.Utilities.Caches.Sessions
{
    public class SessionsCache : UserLevelCache<Session>
    {
        public const int LifetimeMinutes = 20;
        protected override CachingOptions MemoryCacheOptions 
            => CachingOptions.Enabled(TimeSpan.FromMinutes(5)); 
        protected override CachingOptions DistributedCacheOptions
            => CachingOptions.Enabled(TimeSpan.FromMinutes(LifetimeMinutes));
        protected override string CacheKey => "session";

        public SessionsCache(ITelegramUserIdProvider telegramUserIdProvider) : base(telegramUserIdProvider)
        {
        }
    }
}