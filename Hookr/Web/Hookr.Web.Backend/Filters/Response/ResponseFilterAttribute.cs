using System;
using System.Threading.Tasks;
using Hookr.Core.Utilities.Extensions;
using Hookr.Web.Backend.Exceptions;
using Hookr.Web.Backend.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hookr.Web.Backend.Filters.Response
{
    public abstract class ResponseFilterAttribute : ActionFilterAttribute, IAsyncActionFilter
    {
        public sealed override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
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

        private (string Type, string Description, int StatusCode) AnalyzeException(Exception exception)
        {
            Console.WriteLine(exception.ToString());
            var type = ModifyExceptionTypeName(exception);
            return exception switch
            {
                WebAppException webAppException => Wrap(type,AnalyzeWebAppException(webAppException)),
                _ => Wrap(type, exception
                    .Map(DefaultErrorMapper))
            };
        }

        private static string ModifyExceptionTypeName(Exception exception)
            => exception
                .GetType().Name
                .Replace(nameof(Exception), nameof(Error));

        private static (string Type, string Description, int StatusCode) Wrap(string type,
            (string Description, int StatusCode) with)
            => (type, with.Description, with.StatusCode);

        private static (string Description, int StatusCode) DefaultErrorMapper(Exception exception)
            => (exception.Message, 500);

        protected virtual (string Description, int StatusCode)
            AnalyzeWebAppException(WebAppException exception)
            => exception
                .Map(DefaultErrorMapper);

        public sealed override bool IsDefaultAttribute() 
            => base.IsDefaultAttribute();

        public sealed override void OnActionExecuted(ActionExecutedContext context) 
            => base.OnActionExecuted(context);

        public sealed override void OnActionExecuting(ActionExecutingContext context) 
            => base.OnActionExecuting(context);

        public sealed override void OnResultExecuted(ResultExecutedContext context) 
            => base.OnResultExecuted(context);

        public sealed override void OnResultExecuting(ResultExecutingContext context)
            => base.OnResultExecuting(context);

        public sealed override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
            => base.OnResultExecutionAsync(context, next);
    }
}