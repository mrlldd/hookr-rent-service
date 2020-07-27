using System;
using Microsoft.Extensions.Caching.Memory;

namespace HookrTelegramBot.Utilities.Telegram.Caches.UserTemporaryStatus
{
    public class UserTemporaryStatusCache : IUserTemporaryStatusCache
    {
        private readonly IMemoryCache memoryCache;

        public UserTemporaryStatusCache(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public void Set(int userId, UserTemporaryStatus status)
            => memoryCache
                .Set(userId, status, TimeSpan.FromMinutes(1));

        public UserTemporaryStatus Get(int userId)
            => memoryCache
                .TryGetValue(userId, out var result)
                ? (UserTemporaryStatus) result
                : UserTemporaryStatus.Default;
    }
}