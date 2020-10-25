using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Hookr.Core.Config.Telegram;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Utilities.Caches;
using Hookr.Core.Utilities.Extensions;
using Hookr.Core.Utilities.Loaders;
using Hookr.Core.Utilities.Providers;
using Hookr.Web.Backend.Config;
using Hookr.Web.Backend.Exceptions.Auth;
using Hookr.Web.Backend.Operations.Base;
using Hookr.Web.Backend.Utilities;
using Hookr.Web.Backend.Utilities.Caches.Sessions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Hookr.Web.Backend.Operations.Queries.Auth
{
    public class ConfirmAuthQueryHandler : QueryHandler<ConfirmAuthQuery, ConfirmAuthQueryHandler.JwtInfo>
    {
        private readonly ITelegramConfig telegramConfig;
        private readonly ITelegramUserIdProvider telegramUserIdProvider;
        private readonly IJwtConfig jwtConfig;
        private readonly ICacheProvider cacheProvider;
        private readonly ILoaderProvider loaderProvider;

        public ConfirmAuthQueryHandler(ITelegramConfig telegramConfig,
            ITelegramUserIdProvider telegramUserIdProvider,
            IJwtConfig jwtConfig,
            ICacheProvider cacheProvider,
            ILoaderProvider loaderProvider)
        {
            this.telegramConfig = telegramConfig;
            this.telegramUserIdProvider = telegramUserIdProvider;
            this.jwtConfig = jwtConfig;
            this.cacheProvider = cacheProvider;
            this.loaderProvider = loaderProvider;
        }

        public class JwtInfo
        {
            public string Token { get; set; }
            
            public TelegramUserStates Role { get; set; }
        }

        public override Task<JwtInfo> ExecuteQueryAsync(ConfirmAuthQuery query) =>
            VerifyHashes(query)
                ? Authenticate(query)
                : throw new TelegramNotAuthenticatedException();

        private bool VerifyHashes(ConfirmAuthQuery query)
        {
            using var sha256 = SHA256.Create();
            var secretKey = sha256
                .ComputeHash(telegramConfig.Token.Utf8Bytes());
            using var hmacsha256 = new HMACSHA256(secretKey);
            return hmacsha256
                .ComputeHash(DataCheckStringFormatter<ConfirmAuthQuery>.Instance
                    .Format(query, nameof(ConfirmAuthQuery.Hash))
                    .Utf8Bytes())
                .Map(dataCheckStringHash =>
                    new StringBuilder(dataCheckStringHash.Length * 2)
                        .SideEffect(builder => dataCheckStringHash
                            .ForEach(b => builder.AppendFormat("{0:x2}", b))
                        )
                        .ToString()
                        .Equals(query.Hash)
                );
        }

        private async Task<JwtInfo> Authenticate(ConfirmAuthQuery query)
        {
            telegramUserIdProvider.Set(query.Id);
            var user = await loaderProvider
                .Get<int, TelegramUser>()
                .GetOrLoadAsync(query.Id);
            var sessionsCache = cacheProvider
                .UserLevel<Session>();
            var session = SessionFactory(user);
            await sessionsCache.SetAsync(session);
            var claims = new[]
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.State.ToString("G")),
                new Claim(nameof(Session.Id), session.Id.ToString()),
                new Claim(nameof(Session.Key), session.Key.ToString())
            };
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
                Role = user.State
            };
        }

        private static Session SessionFactory(TelegramUser user)
            => new Session
            {
                Key = Guid.NewGuid(),
                Id = user.Id,
                State = user.State,
                Username = user.Username,
                LastUpdatedAt = user.LastUpdatedAt
            };
    }
}