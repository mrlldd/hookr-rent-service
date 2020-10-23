using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Hookr.Core.Utilities.Extensions;
using static System.Linq.Expressions.Expression;

namespace Hookr.Web.Backend.Utilities
{
    public class DataCheckStringFormatter<T>
    {
        private readonly IDictionary<string, (string JsonPropertyName, Delegate Selector)> nameToSelectorMap = typeof(T)
            .GetProperties()
            .Select(x =>
            {
                var name = x.Name;
                var jsonName = TryGetJsonPropertyName(x);
                var parameter = Parameter(typeof(T));
                var expression = Lambda(typeof(Func<,>)
                        .MakeGenericType(typeof(T), x.PropertyType), Property(parameter, name), parameter)
                    .Compile();
                return new
                {
                    Name = name,
                    JsonName = string.IsNullOrEmpty(jsonName)
                        ? name.ToLower()
                        : jsonName,
                    Expression = expression
                };
            })
            .OrderBy(x => x.JsonName)
            .ToDictionary(x => x.Name, x => (x.JsonName, x.Expression));

        public static DataCheckStringFormatter<T> Instance { get; } = new DataCheckStringFormatter<T>();

        private DataCheckStringFormatter()
        {
        }

        public string Format(T source, params string[] except)
            => nameToSelectorMap
                .Where(x => !except.Contains(x.Key))
                .Select(x => $"{x.Value.JsonPropertyName}={x.Value.Selector.DynamicInvoke(source)}")
                .Map(x => new StringBuilder().AppendJoin("\n", x))
                .ToString();

        private static string TryGetJsonPropertyName(MemberInfo propertyInfo)
            => propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name;
    }
}