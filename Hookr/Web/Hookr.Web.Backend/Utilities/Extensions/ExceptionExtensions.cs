using System;

namespace Hookr.Web.Backend.Utilities.Extensions
{
    public static class ExceptionExtensions
    {
        public static Exception Deepest(this Exception exception)
            => exception is AggregateException aggregated
                ? Deepest(aggregated.InnerException)
                : exception;
    }
}