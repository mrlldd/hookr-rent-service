using System;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Utilities.Extensions;
using Hookr.Core.Utilities.Providers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Core.Utilities.Loaders
{
    public class CachingLoaderDispatcher
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly ITelegramUserIdProvider telegramUserIdProvider;

        public CachingLoaderDispatcher(
            IServiceProvider serviceProvider,
            IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            ITelegramUserIdProvider telegramUserIdProvider)
        {
            this.serviceProvider = serviceProvider;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.telegramUserIdProvider = telegramUserIdProvider;
        }

        public Task<TResult> GetOrLoadAsync<TArgs, TResult>(TArgs args,
            bool omitCache = false,
            CancellationToken token = default) where TResult : class
            => serviceProvider
                .GetService<CachingLoader<TArgs, TResult>>()
                .SideEffect(x => x
                    .Populate(memoryCache, distributedCache, telegramUserIdProvider)
                )
                .GetOrLoadAsync(args, omitCache, token);
    }
}