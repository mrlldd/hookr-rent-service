using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Hookr.Core.Utilities.Caching
{
    public interface ICaching<T>
    {
        void Populate(IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            ILogger<ICaching<T>> logger);
    }
}