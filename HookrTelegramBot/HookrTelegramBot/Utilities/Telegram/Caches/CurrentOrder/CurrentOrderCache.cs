using System;
using Microsoft.Extensions.Caching.Memory;

namespace HookrTelegramBot.Utilities.Telegram.Caches.CurrentOrder
{
    public class CurrentOrderCache : ICurrentOrderCache
    {
        private readonly IMemoryCache memoryCache;
        private const int TimeoutMinutes = 3;
        private const string KeyFormat = "{0}co";

        public CurrentOrderCache(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public void Set(int userId, int orderId)
            => memoryCache.Set(string.Format(KeyFormat, userId), orderId, TimeSpan.FromMinutes(TimeoutMinutes));

        public int? Get(int userId)
            => memoryCache.TryGetValue(string.Format(KeyFormat, userId), out var result)
                ? (int) result
                : (int?) null;
    }
}