using System;
using Hookr.Core.Utilities.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace Hookr.Telegram.Utilities.Telegram.Caches.UserTemporaryStatus
{
    public class UserTemporaryStatusCache : Cache<UserTemporaryStatus>, IUserTemporaryStatusCache
    {
        private const int TimeoutMinutes = 3;

        protected override CachingOptions MemoryCacheOptions { get; } =
            new CachingOptions(true, TimeSpan.FromMinutes(TimeoutMinutes));
        protected override CachingOptions DistributedCacheOptions { get; } =
            new CachingOptions(false, TimeSpan.Zero);

        protected override string CacheKey => "temporaryStatus";
    }
}