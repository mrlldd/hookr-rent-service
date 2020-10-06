using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Utilities.Extensions;
using Hookr.Core.Utilities.Providers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Hookr.Core.Utilities.Caching
{
    public abstract class Caching<T> where T : class
    {
        private IMemoryCache MemoryCache { get; set; }
        private ITelegramUserIdProvider TelegramUserIdProvider { get; set; }
        private IDistributedCache DistributedCache { get; set; }
        
        protected abstract CachingOptions MemoryCacheOptions { get; }
        protected abstract CachingOptions DistributedCacheOptions { get; }
        protected abstract string CacheKey { get; }

        internal void Populate(IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            ITelegramUserIdProvider telegramUserIdProvider)
        {
            MemoryCache = memoryCache;
            DistributedCache = distributedCache;
            TelegramUserIdProvider = telegramUserIdProvider;
        }

        private string CacheKeyFactory(string suffix) 
            => string.Join(":",
                TelegramUserIdProvider.ProvidedValue,
                CacheKey,
                suffix);

        protected async Task PerformCachingAsync(T data, string keySuffix, CancellationToken token = default)
        {
            var key = CacheKeyFactory(keySuffix);
            if (MemoryCacheOptions.IsCaching)
            {
                MemoryCache.Set(key, data, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = MemoryCacheOptions.Timeout
                });
            }
            
            token.ThrowIfCancellationRequested();
            
            if (DistributedCacheOptions.IsCaching)
            {
                await DistributedCache.SetStringAsync(key, data.ToJson(), new DistributedCacheEntryOptions
                {
                    SlidingExpiration = DistributedCacheOptions.Timeout
                }, token);
            }
        }

        protected async Task<T?> TryGetFromCacheAsync(string keySuffix, CancellationToken token = default)
        {
            var key = CacheKeyFactory(keySuffix);
            if (MemoryCacheOptions.IsCaching && MemoryCache.TryGetValue<T>(key, out var inMemory))
            {
                return inMemory;
            }

            token.ThrowIfCancellationRequested();
            if (!DistributedCacheOptions.IsCaching)
            {
                return null;
            }

            var inDistributed = await DistributedCache.GetStringAsync(key, token);

            if (string.IsNullOrEmpty(inDistributed))
            {
                return null;
            }
            try
            {
                var deserialized = JsonSerializer.Deserialize<T>(inDistributed);
                if (MemoryCacheOptions.IsCaching)
                {
                    MemoryCache.Set(key, deserialized, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = MemoryCacheOptions.Timeout
                    });
                }

                return deserialized;
            }
            catch
            {
                if (MemoryCacheOptions.IsCaching)
                {
                    MemoryCache.Remove(key);
                }
            }

            return null;
        }
    }
}