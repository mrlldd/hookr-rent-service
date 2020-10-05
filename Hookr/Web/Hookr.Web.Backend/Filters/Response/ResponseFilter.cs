using System;
using System.Threading.Tasks;
using Hookr.Web.Backend.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hookr.Web.Backend.Filters.Response
{
    public class ResponseFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executed = await next();
            var traceId = context.HttpContext.TraceIdentifier;
            if (executed.Exception != null && !executed.ExceptionHandled)
            {
                var (type, description, statusCode) = AnalyzeException(executed.Exception.Deepest());
                executed.Result = new ObjectResult(new Error
                {
                    TraceId = traceId,
                    Description = description,
                    Type = type
                })
                {
                    StatusCode = statusCode
                };
                executed.ExceptionHandled = true;
                return;
            }

            switch (executed.Result)
            {
                case ObjectResult objectResult:
                    objectResult.Value = new Success<object>
                    {
                        TraceId = traceId,
                        Data = objectResult.Value
                    };
                    break;
                case EmptyResult _:
                    executed.Result = new ObjectResult(new Success
                    {
                        TraceId = traceId
                    });
                    break;
            }
        }

        private static (string Type, string Description, int StatusCode) AnalyzeException(Exception exception)
        {
            var type = ModifyExceptionTypeName(exception);
            return exception switch
            {
                _ => (type, exception.Message, 500)
            };
        }

        private static string ModifyExceptionTypeName(Exception exception)
            => exception
                .GetType().Name
                .Replace(nameof(Exception), nameof(Error));
    }
}