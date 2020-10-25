using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Hookr.Core.Repository.Context.Entities.Base;

namespace Hookr.Web.Backend.Utilities.Jwt
{
    public class JwtPayloadReader : JwtPayloadInteractor
    {
        public JwtPayloadReadResult Read(JwtSecurityToken token)
        {
            var payloadClaims = token.Payload.Claims
                .ToArray();
            var (rawKey, rawId, rawRole) = (
                FindInClaims(payloadClaims, KeyType),
                FindInClaims(payloadClaims, IdType),
                FindInClaims(payloadClaims, RoleType)
            );

            return !Guid.TryParse(rawKey, out var key)
                   || !int.TryParse(rawId, out var id)
                   || !Enum.TryParse<TelegramUserStates>(rawRole, out var role)
                ? new JwtPayloadReadResult
                {
                    Payload = new JwtPayload
                    {
                        Id = default,
                        Key = default,
                        Role = default
                    }
                }
                : new JwtPayloadReadResult
                {
                    Success = true,
                    Payload = new JwtPayload
                    {
                        Id = id,
                        Key = key,
                        Role = role
                    }
                };
        }

        private static string FindInClaims(IEnumerable<Claim> claims, string type)
            => claims
                .FirstOrDefault(x => x.Type.Equals(type))?.Value;
    }
}