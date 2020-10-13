using System;
using System.Collections.Generic;
using Hookr.Core.Utilities.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hookr.Core.Utilities.Caching
{
    internal abstract class CachingProvider
    {
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly IServiceProvider serviceProvider;
        private readonly IDictionary<Type, object> scopedServicesCache = new Dictionary<Type, object>();

        protected CachingProvider(IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            IServiceProvider serviceProvider)
        {
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.serviceProvider = serviceProvider;
        }

        private void Populate<T, TCached>(T target) where T : ICaching<TCached>
            => (target as Caching<TCached>)?
                .Populate(memoryCache,
                    distributedCache,
                    serviceProvider.GetRequiredService<ILogger<ICaching<TCached>>>());

        protected T InternalGet<T, TCached>() where T : ICaching<TCached>
            => scopedServicesCache.TryGetValue(typeof(T), out var raw)
               && raw is T service
                ? service
                : serviceProvider
                    .GetRequiredService<T>()
                    .SideEffect(Populate<T, TCached>)
                    .SideEffect(x => scopedServicesCache[typeof(T)] = x);
    }
}