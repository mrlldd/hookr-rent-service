using System.Threading;
using System.Threading.Tasks;

namespace Hookr.Core.Utilities.Caching
{
    public abstract class Cache<T> : Caching<T>, ICache<T>
    {
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual string DefaultKeySuffix { get; } = "single";

        public Task SetAsync(T value, CancellationToken token = default)
            => PerformCachingAsync(value, DefaultKeySuffix, token);

        public Task<T> GetAsync(CancellationToken token = default)
            => TryGetFromCacheAsync(DefaultKeySuffix, token);
    }
}