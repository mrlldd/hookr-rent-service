using System;
using System.Collections.Generic;
using System.Linq;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Repository.Context.Entities.Products;

namespace HookrTelegramBot.Utilities.Extensions
{
    public static class ProductExtensions
    {
        public static string AggregateListString(this IEnumerable<Product> products, string format,
            params Func<Product, object>[] argsSelectors)
            => products
                .Select((x, index) => string.Format(format, new List<object>
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