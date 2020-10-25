using System;
using System.Linq;
using System.Threading.Tasks;
using Hookr.Web.Backend.Exceptions.Auth;
using Hookr.Web.Backend.Operations.Base;
using Hookr.Web.Backend.Utilities;

namespace Hookr.Web.Backend.Operations.Queries.Auth
{
    public class GetRefreshTokenQueryHandler : QueryHandler<GetRefreshTokenQuery, Guid>
    {
        private readonly IUserContextAccessor userContextAccessor;

        public GetRefreshTokenQueryHandler(IUserContextAccessor userContextAccessor)
        {
            this.userContextAccessor = userContextAccessor;
        }

        public override Task<Guid> ExecuteQueryAsync(GetRefreshTokenQuery query)
        {
            var now = DateTime.UtcNow;
            var token = userContextAccessor.Context
                .RefreshTokens
                .FirstOrDefault(x => !x.Used && x.ExpiresAt > now);
            return token == null
                ? throw new MissingRefreshTokenException()
                : Task.FromResult(token.Value);
        }
    }
}