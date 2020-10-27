using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Hookr.Core.Config.Telegram;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using JwtPayload = Hookr.Web.Backend.Utilities.Jwt.JwtPayload;

namespace Hookr.Web.Backend.Operations.Queries.Auth
{
    public class CreateSessionQueryHandler : SessionFactoryQueryHandler<CreateSessionQuery>
    {
        private readonly ITelegramConfig telegramConfig;
        private readonly ITelegramUserIdProvider telegramUserIdProvider;
        private readonly IHookrRepository hookrRepository;

        public CreateSessionQueryHandler(ITelegramConfig telegramConfig,
            ITelegramUserIdProvider telegramUserIdProvider,
            IJwtConfig jwtConfig,
            ICacheProvider cacheProvider,
            IHookrRepository hookrRepository)
            : base(cacheProvider,
                jwtConfig)
        {
            this.telegramConfig = telegramConfig;
            this.telegramUserIdProvider = telegramUserIdProvider;
            this.hookrRepository = hookrRepository;
        }


        public override Task<AuthResult> ExecuteQueryAsync(CreateSessionQuery query) =>
            VerifyHashes(query)
                ? AuthenticateAsync(query)
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

        private async Task<AuthResult> AuthenticateAsync(TelegramUserDto dto)
        {
            telegramUserIdProvider.Set(dto.Id);
            var user = await GetOrCreateTelegramUser(dto);
            return await CreateAndSaveSessionAsync(user);
        }

        private async Task<TelegramUser> GetOrCreateTelegramUser(TelegramUserDto dto)
        {
            var user = await hookrRepository
                .ReadAsync((context, token) => context.TelegramUsers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == dto.Id, token),
                    Token);
            if (user != null)
            {
                await UpdateTelegramUserAsync(hookrRepository.Context.Update(user).Entity, dto);
                return user;
            }

            var createdUser = new TelegramUser
            {
                Id = dto.Id,
                State = TelegramUserStates.Default
            };
            await UpdateTelegramUserAsync(hookrRepository.Context.Add(createdUser).Entity,
                dto);
            return createdUser;
        }

        private async Task UpdateTelegramUserAsync(TelegramUser user,
            TelegramUserDto dto)
        {
            user.Username = dto.Username;
            user.FirstName = dto.FirstName;
            user.PhotoUrl = dto.PhotoUrl;
            user.LastUpdatedAt = DateTime.UtcNow;
            await hookrRepository.SaveChangesAsync();
        }
    }
}