using System;
using System.Collections.Generic;
using System.Linq;

namespace Hookr.Core.Utilities.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T>? source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static IEnumerable<T> Linear<T>(this IEnumerable<IEnumerable<T>> dimensional)
            => dimensional
                .SelectMany(x => x);
    }
}