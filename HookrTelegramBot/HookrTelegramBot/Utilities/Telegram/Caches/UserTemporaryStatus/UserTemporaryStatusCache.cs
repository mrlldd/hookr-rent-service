using System;
using Microsoft.Extensions.Caching.Memory;

namespace HookrTelegramBot.Utilities.Telegram.Caches.UserTemporaryStatus
{
    public class UserTemporaryStatusCache : IUserTemporaryStatusCache
    {
        private const int TimeoutMinutes = 3;
        private readonly IMemoryCache memoryCache;
        private const string KeyFormat = "{0}ts";

        public UserTemporaryStatusCache(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public void Set(int userId, UserTemporaryStatus status)
            => memoryCache
                .Set(string.Format(KeyFormat, userId), status, TimeSpan.FromMinutes(TimeoutMinutes));

        public UserTemporaryStatus Get(int userId)
            => memoryCache
                .TryGetValue(string.Format(KeyFormat, userId), out var result)
                ? (UserTemporaryStatus) result
                : UserTemporaryStatus.Default;
    }
}