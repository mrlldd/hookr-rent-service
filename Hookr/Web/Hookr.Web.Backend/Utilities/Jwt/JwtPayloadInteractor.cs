using System.Security.Claims;
using Hookr.Web.Backend.Utilities.Caches.Sessions;

namespace Hookr.Web.Backend.Utilities.Jwt
{
    public class JwtPayloadInteractor
    {
        protected const string KeyType = nameof(Session.Key);
        protected const string RoleType = ClaimsIdentity.DefaultRoleClaimType;
        protected const string IdType = nameof(Session.Id);
    }
}