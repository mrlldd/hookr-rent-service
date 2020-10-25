using System;
using Hookr.Core.Utilities.Caches.Leveled;
using Hookr.Core.Utilities.Caching;
using Hookr.Core.Utilities.Providers;

namespace Hookr.Web.Backend.Utilities.Caches.Sessions
{
    public class SessionsCache : UserLevelCache<Session>
    {
        protected override CachingOptions MemoryCacheOptions 
            => CachingOptions.Enabled(TimeSpan.FromMinutes(10)); 
        protected override CachingOptions DistributedCacheOptions
            => CachingOptions.Enabled(TimeSpan.FromMinutes(60));
        protected override string CacheKey => "session";

        public SessionsCache(ITelegramUserIdProvider telegramUserIdProvider) : base(telegramUserIdProvider)
        {
        }
    }
}