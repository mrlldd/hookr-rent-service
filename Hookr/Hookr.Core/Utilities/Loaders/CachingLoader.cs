using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Utilities.Caching;
using Hookr.Core.Utilities.Extensions;

namespace Hookr.Core.Utilities.Loaders
{
    public abstract class CachingLoader<TArgs, TResult> : Caching<TResult> where TResult : class
    {
        public async Task<TResult> GetOrLoadAsync(TArgs args, bool omitCacheOnLoad = false, CancellationToken token = default)
        {
            var keySuffix = CacheKeySuffixFactory(args);
            if (!omitCacheOnLoad)
            {
                var inCache = await TryGetFromCacheAsync(keySuffix, token);
                if (inCache != null)
                {
                    return inCache;
                }
            }

            var loaded = await LoadAsync(args, token);
            return await loaded
                .SideEffectAsync(x => PerformCachingAsync(x, keySuffix, token));
        }

        protected abstract Task<TResult> LoadAsync(TArgs args, CancellationToken token = default);

        protected abstract string CacheKeySuffixFactory(TArgs args);
    }
}