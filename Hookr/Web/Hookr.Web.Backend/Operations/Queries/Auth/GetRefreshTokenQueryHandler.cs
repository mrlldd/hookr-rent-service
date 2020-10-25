using System;
using System.Linq;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context;
using Hookr.Core.Repository.Context.Entities;
using Hookr.Core.Utilities.Extensions;
using Hookr.Web.Backend.Exceptions.Auth;
using Hookr.Web.Backend.Operations.Base;
using Hookr.Web.Backend.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Hookr.Web.Backend.Operations.Queries.Auth
{
    public class GetRefreshTokenQueryHandler : QueryHandler<GetRefreshTokenQuery, Guid>
    {
        private readonly IUserContextAccessor userContextAccessor;
        private readonly HookrContext hookrContext;
        private const int RefreshExpirationDays = 14;

        public GetRefreshTokenQueryHandler(IUserContextAccessor userContextAccessor, 
            HookrContext hookrContext)
        {
            this.userContextAccessor = userContextAccessor;
            this.hookrContext = hookrContext;
        }

        public override Task<Guid> ExecuteQueryAsync(GetRefreshTokenQuery query)
        {
            var session = userContextAccessor.Context;
            var token = RefreshTokenFactory()
                .SideEffect(x => x.UserId = session.Id);
            
            hookrContext.RefreshTokens
                .Add(token);
            return Task.FromResult(token.Value);
        }
        
        
        private static RefreshToken RefreshTokenFactory()
            => new RefreshToken
            {
                ExpiresAt = DateTime.UtcNow.AddDays(RefreshExpirationDays),
                Value = Guid.NewGuid(),
                Used = false
            };
    }
}