using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Utilities.Caches;
using Hookr.Core.Utilities.Extensions;
using Hookr.Core.Utilities.Providers;
using Hookr.Web.Backend.Utilities.Caches.Sessions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        public async Task InvokeAsync(HttpContext httpContext,
            ILogger<JwtReaderMiddleware> logger,
            ICacheProvider cacheProvider,
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

            var (tokenReadSuccess, decodedToken) = TryReadToken(header, logger);
            if (!tokenReadSuccess)
            {
                await next(httpContext);
                return;
            }

            var payloadClaims = decodedToken.Payload.Claims
                .ToArray();
            var (rawKey, rawId, rawRole) = (
                FindInClaims(payloadClaims, nameof(Session.Key)),
                FindInClaims(payloadClaims, nameof(TelegramUser.Id)),
                FindInClaims(payloadClaims, ClaimsIdentity.DefaultRoleClaimType)
            );

            if (!Guid.TryParse(rawKey, out var key)
                || !int.TryParse(rawId, out var id)
                || !Enum.TryParse<TelegramUserStates>(rawRole, out var role))
            {
                httpContext.Response.StatusCode = 401;
                return;
            }

            telegramUserIdProvider.Set(id);
            var cachedSession = await cacheProvider
                .UserLevel<Session>()
                .GetAsync();
            if (!cachedSession.Key.Equals(key)
                || !cachedSession.Id.Equals(id)
                || !cachedSession.State.Equals(role))
            {
                httpContext.Response.StatusCode = 401;
                return;
            }

            await next(httpContext);
        }

        private static (bool Success, JwtSecurityToken DecodedToken) TryReadToken(string token, ILogger logger)
        {
            try
            {
                return (true, token
                    .Replace(Bearer, string.Empty)
                    .Trim()
                    .Map(new JwtSecurityTokenHandler().ReadJwtToken));
            }
            catch (Exception e)
            {
                logger.LogDebug(e, "Failed to read JWT token.");
                return (false, null);
            }
        }

        private static string FindInClaims(IEnumerable<Claim> claims, string type)
            => claims
                .FirstOrDefault(x => x.Type.Equals(type))?.Value;
    }
}