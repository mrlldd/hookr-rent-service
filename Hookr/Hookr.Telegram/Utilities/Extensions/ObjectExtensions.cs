using System;
using Newtonsoft.Json;

namespace Hookr.Telegram.Utilities.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
            => JsonConvert.SerializeObject(obj);

        public static T SideEffect<T>(this T obj, Action<T> effect)
        {
            effect(obj);
            return obj;
        }
    }
}