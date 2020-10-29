using System.Security.Claims;

namespace Hookr.Web.Backend.Utilities.Jwt
{
    public class JwtPayloadWriter : JwtPayloadInteractor
    {
        public Claim[] WriteClaims(JwtPayload payload) 
            => new[]
            {
                new Claim(RoleType, payload.Role.ToString("G")),
                new Claim(IdType, payload.Id.ToString()),
                new Claim(KeyType, payload.Key.ToString())
            };
    }
}