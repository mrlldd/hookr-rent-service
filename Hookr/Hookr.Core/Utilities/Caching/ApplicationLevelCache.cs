using System.Collections.Generic;

namespace Hookr.Core.Utilities.Caching
{
    public abstract class ApplicationLevelCache<T> : Cache<T>
    {
        protected sealed override IEnumerable<object> CacheKeyPrefixesFactory()
            => new List<object>
            {
                "app"
            };
    }
}