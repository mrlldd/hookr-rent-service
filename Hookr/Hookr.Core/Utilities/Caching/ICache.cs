using System.Threading;
using System.Threading.Tasks;

namespace Hookr.Core.Utilities.Caching
{
    public interface ICache<T>
    {
        Task SetAsync(T value, CancellationToken token = default);
        Task<T> GetAsync(CancellationToken token = default);
    }
}