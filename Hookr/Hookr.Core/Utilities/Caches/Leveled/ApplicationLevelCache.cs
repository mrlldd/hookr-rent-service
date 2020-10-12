using System.Collections.Generic;

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