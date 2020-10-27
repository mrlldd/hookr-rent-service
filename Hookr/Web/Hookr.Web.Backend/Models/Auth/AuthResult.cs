using Hookr.Core.Repository.Context.Entities.Base;

namespace Hookr.Web.Backend.Models.Auth
{
    public class AuthResult
    {
        public string Token { get; set; }
            
        public TelegramUserStates Role { get; set; }
        public TelegramUserDto User { get; set; }
        public int Lifetime { get; set; }
    }
}