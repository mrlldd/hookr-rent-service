using System;
using System.Linq;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Utilities.Caches;
using Hookr.Core.Utilities.Loaders;
using Hookr.Core.Utilities.Providers;
using Hookr.Web.Backend.Config;
using Hookr.Web.Backend.Exceptions.Auth;
using Hookr.Web.Backend.Models.Auth;
using Hookr.Web.Backend.Operations.Base;
using Hookr.Web.Backend.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Hookr.Web.Backend.Operations.Queries.Auth
{
    public class RefreshSessionQueryHandler : SessionFactoryQueryHandler<RefreshSessionQuery>
    {
        private readonly ITelegramUserIdProvider telegramUserIdProvider;
        private readonly IHookrRepository hookrRepository;
        public RefreshSessionQueryHandler(
            ITelegramUserIdProvider telegramUserIdProvider,
            IHookrRepository hookrRepository,
            ILoaderProvider loaderProvider,
            ICacheProvider cacheProvider,
            IJwtConfig jwtConfig) : base(loaderProvider, cacheProvider, jwtConfig)
        {
            this.telegramUserIdProvider = telegramUserIdProvider;
            this.hookrRepository = hookrRepository;
        }

        public override async Task<JwtInfo> ExecuteQueryAsync(RefreshSessionQuery query)
        {
            var now = DateTime.UtcNow;
            var token = await hookrRepository
                .ReadAsync((context, cancellationToken) => context
                    .RefreshTokens
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
            var result = await CreateAndSaveSessionAsync(token.UserId);
            token.Used = true;
            return result;
        }

    
    }
}