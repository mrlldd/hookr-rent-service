using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Hookr.Web.Backend.Middleware
{
    public class JwtCryptorMiddleware
    {
        private readonly RequestDelegate next;

        public JwtCryptorMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task InvokeAsync(HttpContext httpContext)
        {
            var header = httpContext.Request.Headers["Authorization"];
            Console.WriteLine(JsonConvert.SerializeObject(header));
            return next(httpContext);
        }
    }
}