using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Utilities.Caches;
using Hookr.Core.Utilities.Extensions;
using Hookr.Core.Utilities.Loaders;
using Hookr.Web.Backend.Config;
using Hookr.Web.Backend.Models.Auth;
using Hookr.Web.Backend.Operations.Base;
using Hookr.Web.Backend.Utilities.Caches.Sessions;
using Hookr.Web.Backend.Utilities.Jwt;
using Microsoft.IdentityModel.Tokens;
using JwtPayload = Hookr.Web.Backend.Utilities.Jwt.JwtPayload;

namespace Hookr.Web.Backend.Operations.Queries.Auth
{
    public abstract class SessionFactoryQueryHandler<TArgs> : QueryHandler<TArgs, JwtInfo>
    {
        private readonly ILoaderProvider loaderProvider;
        private readonly ICacheProvider cacheProvider;
        private readonly IJwtConfig jwtConfig;

        protected SessionFactoryQueryHandler(ILoaderProvider loaderProvider,
            ICacheProvider cacheProvider,
            IJwtConfig jwtConfig)
        {
            this.loaderProvider = loaderProvider;
            this.cacheProvider = cacheProvider;
            this.jwtConfig = jwtConfig;
        }

        protected async Task<JwtInfo> CreateAndSaveSessionAsync(int telegramUserId)
        {
            var user = await loaderProvider
                .Get<int, TelegramUser>()
                .GetOrLoadAsync(telegramUserId, true, Token);
            var sessionsCache = cacheProvider
                .UserLevel<Session>();
            var session = SessionFactory(user);
            await sessionsCache
                .SetAsync(session, Token);
            var claims = new JwtPayloadWriter()
                .Write(new JwtPayload
                {
                    Id = session.Id,
                    Key = session.Key,
                    Role = session.State
                });
            var now = DateTime.UtcNow;
            return new JwtInfo
            {
                Token = new JwtSecurityToken(jwtConfig.Issuer,
                        jwtConfig.Audience,
                        claims,
                        now,
                        now.AddMinutes(jwtConfig.LifeTimeMinutes),
                        new SigningCredentials(new SymmetricSecurityKey(jwtConfig.Key.Utf8Bytes()), 
                            SecurityAlgorithms.HmacSha256Signature)
                    )
                    .Map(new JwtSecurityTokenHandler().WriteToken),
                Role = session.State
            };
        }
        
        private static Session SessionFactory(TelegramUser user)
            => new Session
            {
                Key = Guid.NewGuid(),
                Id = user.Id,
                State = user.State,
                Username = user.Username,
                RefreshTokens = user.RefreshTokens,
                LastUpdatedAt = user.LastUpdatedAt
            };
    }
}