using System;
using System.Collections.Generic;
using System.Linq;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;

namespace HookrTelegramBot.Utilities.Extensions
{
    public static class OrderedExtensions
    {
        public static string AggregateListString<TProduct>(this IEnumerable<Ordered<TProduct>> orderedProducts,
            string format,
            params Func<Ordered<TProduct>, object>[] argsSelectors) where TProduct : Product
            => orderedProducts
                .Select((x, index) => string
                    .Format(format,
                        new List<object>
                            {
                                index + 1
                            }
                            .Concat(
                                argsSelectors
                                    .Select(selector => selector(x))
                            )
                            .ToArray()
                    )
                )
                .Aggregate((prev, next) => prev + "\n" + next);
    }
}