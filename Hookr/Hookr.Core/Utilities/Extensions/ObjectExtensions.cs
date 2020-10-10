using System;
using System.Text.Json;
using System.Threading.Tasks;
using Hookr.Core.Config.Telegram;

namespace Hookr.Core.Utilities.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object value)
            => JsonSerializer.Serialize(value);

        public static T SideEffect<T>(this T obj, Action<T> effect)
        {
            effect(obj);
            return obj;
        }

        public static async Task<T> SideEffectAsync<T>(this T obj, Func<T, Task> effect)
        {
            await effect(obj);
            return obj;
        }

        public static TResult Map<T, TResult>(this T obj, Func<T, TResult> mapper)
            => mapper(obj);

        public static (T First, TWith Second) Unite<T, TWith>(this T obj, TWith with)
            => (obj, with);
    }
}