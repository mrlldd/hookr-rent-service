using System.Collections.Generic;
using Hookr.Core.Utilities.Caching;

namespace Hookr.Core.Utilities.Caches.Leveled
{
    public abstract class ApplicationLevelCache<T> : Cache<T>, IApplicationLevelCache<T>
    {
        protected sealed override IEnumerable<object> CacheKeyPrefixesFactory()
            => new List<object>
            {
                "app"
            };
    }
}