using System;
using System.Collections.Generic;
using System.Linq;
using Hookr.Telegram.Repository.Context.Entities.Products;

namespace Hookr.Telegram.Utilities.Extensions
{
    public static class ProductExtensions
    {
        public static string AggregateListString(this IEnumerable<Product> products, string format,
            params Func<Product, object>[] argsSelectors)
            => products
                .Select((x, index) => string.Format(format, new object[]
                    {
                        index + 1
                    }
                    .Concat(argsSelectors
                        .Select(selector => selector(x)))
                    .ToArray())
                )
                .Aggregate((prev, next) => prev + "\n" + next);
    }
}