
using System.Diagnostics.CodeAnalysis;

namespace Hookr.Core.Config.Cache
{
    public class CacheConfig : ICacheConfig
    {
        [NotNull]public string? ConnectionString { get; set; }
        public int LinearRetries { get; set; }
        public int KeepAliveSeconds { get; set; }
    }
}