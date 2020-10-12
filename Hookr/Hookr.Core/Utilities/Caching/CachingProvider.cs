using System;
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
            => serviceProvider
                .GetRequiredService<T>()
                .SideEffect(Populate<T, TCached>);
    }
}