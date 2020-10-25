using Hookr.Core.Repository.Context.Entities.Base;

namespace Hookr.Web.Backend.Models.Auth
{
    public class JwtInfo
    {
        public string Token { get; set; }
            
        public TelegramUserStates Role { get; set; }
    }
}