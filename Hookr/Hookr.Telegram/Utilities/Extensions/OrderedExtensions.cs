using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Hookr.Core.Repository.Context.Entities.Products;
using Hookr.Core.Repository.Context.Entities.Products.Ordered;

namespace Hookr.Telegram.Utilities.Extensions
{
    public static class OrderedExtensions
    {
        public static string AggregateListString<TProduct>([AllowNull] this IEnumerable<Ordered<TProduct>> orderedProducts,
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