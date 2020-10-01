using System;
using System.Collections.Generic;
using System.Linq;

namespace HookrTelegramBot.Utilities.Extensions
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

        public static IEnumerable<T> Linear<T>(this IEnumerable<IEnumerable<T>> dimensional)
            => dimensional
                .SelectMany(x => x);
    }
}