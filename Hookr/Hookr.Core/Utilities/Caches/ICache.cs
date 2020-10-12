using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Utilities.Caching;

namespace Hookr.Core.Utilities.Caches
{
    public interface ICache<T> : ICaching<T>
    {
        Task SetAsync(T value, CancellationToken token = default);
        Task<T> GetAsync(CancellationToken token = default);
    }
}