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
using Hookr.Web.Backend.Models.Auth;
using Hookr.Web.Backend.Operations.Base;
using Hookr.Web.Backend.Utilities;
using Hookr.Web.Backend.Utilities.Caches.Sessions;
using Hookr.Web.Backend.Utilities.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using JwtPayload = Hookr.Web.Backend.Utilities.Jwt.JwtPayload;

namespace Hookr.Web.Backend.Operations.Queries.Auth
{
    public class CreateSessionQueryHandler : SessionFactoryQueryHandler<CreateSessionQuery>
    {
        private readonly ITelegramConfig telegramConfig;
        private readonly ITelegramUserIdProvider telegramUserIdProvider;

        public CreateSessionQueryHandler(ITelegramConfig telegramConfig,
            ITelegramUserIdProvider telegramUserIdProvider,
            IJwtConfig jwtConfig,
            ICacheProvider cacheProvider,
            ILoaderProvider loaderProvider) 
            : base(loaderProvider,
                cacheProvider,
                jwtConfig)
        {
            this.telegramConfig = telegramConfig;
            this.telegramUserIdProvider = telegramUserIdProvider;
        }


        public override Task<JwtInfo> ExecuteQueryAsync(CreateSessionQuery query) =>
            VerifyHashes(query)
                ? AuthenticateAsync(query.Id)
                : throw new TelegramNotAuthenticatedException();

        private bool VerifyHashes(CreateSessionQuery query)
        {
            using var sha256 = SHA256.Create();
            var secretKey = sha256
                .ComputeHash(telegramConfig.Token.Utf8Bytes());
            using var hmacsha256 = new HMACSHA256(secretKey);
            return hmacsha256
                .ComputeHash(DataCheckStringFormatter<CreateSessionQuery>.Instance
                    .Format(query, nameof(CreateSessionQuery.Hash))
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

        private Task<JwtInfo> AuthenticateAsync(int telegramUserId)
        {
            telegramUserIdProvider.Set(telegramUserId);
            return CreateAndSaveSessionAsync(telegramUserId);
        }

    }
}