using System;
using Hookr.Web.Backend.Exceptions;
using Hookr.Web.Backend.Exceptions.Auth;

namespace Hookr.Web.Backend.Filters.Response.Auth
{
    public class AuthResponseFilterAttribute : ResponseFilterAttribute
    {
        protected override (string Description, int StatusCode) AnalyzeWebAppException(WebAppException exception) =>
            exception switch
            {
                TelegramNotAuthenticatedException notAuthenticated =>
                (notAuthenticated.Message, 401),
                _ => base.AnalyzeWebAppException(exception)
            };
    }
}