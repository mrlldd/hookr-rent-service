using System;

namespace Hookr.Core.Utilities.Caching
{
    public class CachingOptions
    {
        public CachingOptions(bool shouldCache, TimeSpan timeout)
        {
            IsCaching = shouldCache;
            Timeout = timeout;
        }

        public bool IsCaching { get; }
        public TimeSpan Timeout { get; }
        
        public static readonly CachingOptions False = new CachingOptions(false, TimeSpan.Zero);
    }
}