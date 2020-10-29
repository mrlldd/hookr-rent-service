using System;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Utilities.Caches;
using Hookr.Core.Utilities.Providers;
using Hookr.Web.Backend.Config;
using Hookr.Web.Backend.Exceptions.Auth;
using Hookr.Web.Backend.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Hookr.Web.Backend.Operations.Queries.Auth
{
    public class RefreshSessionQueryHandler : SessionFactoryQueryHandler<RefreshSessionQuery>
    {
        private readonly ITelegramUserIdProvider telegramUserIdProvider;
        private readonly IHookrRepository hookrRepository;
        public RefreshSessionQueryHandler(
            ITelegramUserIdProvider telegramUserIdProvider,
            IHookrRepository hookrRepository,
            ICacheProvider cacheProvider,
            IJwtConfig jwtConfig) : base(cacheProvider, jwtConfig)
        {
            this.telegramUserIdProvider = telegramUserIdProvider;
            this.hookrRepository = hookrRepository;
        }

        public override async Task<AuthResult> ExecuteQueryAsync(RefreshSessionQuery query)
        {
            var now = DateTime.UtcNow;
            var token = await hookrRepository
                .ReadAsync((context, cancellationToken) => context
                    .RefreshTokens
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => !x.Used
                                              && x.ExpiresAt > now
                                              && x.Value
                                                  .Equals(query.RefreshToken),
                        cancellationToken), Token);
            if (token == null)
            {
                throw new RefreshTokenNotFoundException();
            }
            telegramUserIdProvider.Set(token.UserId);
            var result = await CreateAndSaveSessionAsync(token.User);
            token.Used = true;
            await hookrRepository.SaveChangesAsync();
            return result;
        }

    
    }
}