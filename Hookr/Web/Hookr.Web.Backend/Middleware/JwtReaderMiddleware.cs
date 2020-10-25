using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Utilities.Caches;
using Hookr.Core.Utilities.Extensions;
using Hookr.Core.Utilities.Providers;
using Hookr.Web.Backend.Utilities;
using Hookr.Web.Backend.Utilities.Caches.Sessions;
using Hookr.Web.Backend.Utilities.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Hookr.Web.Backend.Middleware
{
    public class JwtReaderMiddleware
    {
        private readonly RequestDelegate next;
        private const string Bearer = "Bearer";

        public JwtReaderMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        
        
        // ReSharper disable once UnusedMember.Global
        public async Task InvokeAsync(HttpContext httpContext,
            ILogger<JwtReaderMiddleware> logger,
            ICacheProvider cacheProvider,
            IUserContextAccessor userContextAccessor,
            IHookrRepository hookrRepository,
            ITelegramUserIdProvider telegramUserIdProvider
        )
        {
            var header = httpContext.Request.Headers["Authorization"]
                .FirstOrDefault(x => x.StartsWith(Bearer));
            if (string.IsNullOrEmpty(header))
            {
                await next(httpContext);
                return;
            }

            var readResult = header
                .Replace(Bearer, string.Empty)
                .Trim()
                .Map(new JwtSecurityTokenHandler().ReadJwtToken)
                .Map(new JwtPayloadReader().Read);


            if (!readResult.Success)
            {
                httpContext.Response.StatusCode = 401;
                return;
            }

            var payload = readResult.Payload;
            telegramUserIdProvider.Set(payload.Id);

            var cachedSession = await cacheProvider
                .UserLevel<Session>()
                .GetAsync();
            if (cachedSession == null
                || !cachedSession.Key.Equals(payload.Key)
                || !cachedSession.Id.Equals(payload.Id)
                || !cachedSession.State.Equals(payload.Role))
            {
                httpContext.Response.StatusCode = 401;
                return;
            }
            logger
                .LogInformation("Found valid session for {TelegramUserId} in caches", payload.Id);
            userContextAccessor
                .SetContext(cachedSession);
            await next(httpContext);
            await userContextAccessor
                .SaveChangesAsync();
            await hookrRepository
                .SaveChangesAsync();
        }


    }
}