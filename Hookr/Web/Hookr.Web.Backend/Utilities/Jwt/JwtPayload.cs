using System;
using Hookr.Core.Repository.Context.Entities.Base;

namespace Hookr.Web.Backend.Utilities.Jwt
{
    public class JwtPayload
    {
        public Guid Key { get; set; }
        public int Id { get; set; }
        public TelegramUserStates Role { get; set; }
    }
}