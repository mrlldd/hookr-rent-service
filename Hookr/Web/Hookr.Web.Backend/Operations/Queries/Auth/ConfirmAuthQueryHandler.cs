using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Hookr.Core.Config.Telegram;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Utilities.Extensions;
using Hookr.Core.Utilities.Loaders;
using Hookr.Core.Utilities.Providers;
using Hookr.Web.Backend.Config;
using Hookr.Web.Backend.Exceptions.Auth;
using Hookr.Web.Backend.Operations.Base;
using Hookr.Web.Backend.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Hookr.Web.Backend.Operations.Queries.Auth
{
    public class ConfirmAuthQueryHandler : QueryHandler<ConfirmAuthQuery, ConfirmAuthQueryHandler.JwtTokens>
    {
        private readonly ITelegramConfig telegramConfig;
        private readonly ITelegramUserIdProvider telegramUserIdProvider;
        private readonly IJwtConfig jwtConfig;
        private readonly ILoaderProvider loaderProvider;

        public ConfirmAuthQueryHandler(ITelegramConfig telegramConfig,
            ITelegramUserIdProvider telegramUserIdProvider,
            IJwtConfig jwtConfig,
            ILoaderProvider loaderProvider)
        {
            this.telegramConfig = telegramConfig;
            this.telegramUserIdProvider = telegramUserIdProvider;
            this.jwtConfig = jwtConfig;
            this.loaderProvider = loaderProvider;
        }

        public class JwtTokens
        {
            public string Token { get; set; }
            public string Refresh { get; set; }
        }

        public override Task<JwtTokens> ExecuteQueryAsync(ConfirmAuthQuery query) =>
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

        private async Task<JwtTokens> Authenticate(ConfirmAuthQuery query)
        {
            telegramUserIdProvider.Set(query.Id);
            var user = await loaderProvider
                .Get<int, TelegramUser>()
                .GetOrLoadAsync(query.Id);
            user.Username = query.Username;
            var claims = new[]
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.State.ToString("G")),
                new Claim("Id", user.Id.ToString()),
            };
            var now = DateTime.UtcNow;
            return new JwtTokens
            {
                Token = new JwtSecurityToken(jwtConfig.Issuer,
                        jwtConfig.Audience,
                        claims,
                        now,
                        now.AddMinutes(5),
                        new SigningCredentials(new SymmetricSecurityKey(jwtConfig.Key.Utf8Bytes()),
                            SecurityAlgorithms.HmacSha256)
                    )
                    .Map(new JwtSecurityTokenHandler().WriteToken),
                Refresh = DateTime.Now.ToString(CultureInfo.InvariantCulture) //todo
            };
        }
    }
}