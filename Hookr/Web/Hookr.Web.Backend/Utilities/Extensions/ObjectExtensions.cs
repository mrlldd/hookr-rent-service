using System;

namespace Hookr.Web.Backend.Utilities.Extensions
{
    public static class ObjectExtensions
    {
        public static TResult Map<T, TResult>(this T obj, Func<T, TResult> functor)
            => functor(obj);

        public static T SideEffect<T>(this T obj, Action<T> effect)
        {
            effect(obj);
            return obj;
        }
    }
}