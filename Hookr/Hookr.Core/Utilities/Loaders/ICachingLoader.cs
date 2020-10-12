using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Utilities.Caching;

namespace Hookr.Core.Utilities.Loaders
{
    public interface ICachingLoader<in TArgs, TResult> : ICaching<TResult>
    {
        Task<TResult> GetOrLoadAsync(TArgs args, bool omitCacheOnLoad = false, CancellationToken token = default);
    }
}