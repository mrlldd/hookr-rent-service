using System;
using System.Collections.Generic;
using System.Linq;

namespace Hookr.Web.Backend.Utilities.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
        
        public static IEnumerable<T> WithNoAny<T, TExclude>(this IEnumerable<T> source)
            => source
                .Where(x => !(x is TExclude));
    }
}