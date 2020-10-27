using System;
using Hookr.Core.Repository.Context.Entities.Base;

namespace Hookr.Web.Backend.Utilities.Caches.Sessions
{
    public class Session
    {
        public int Id { get; set; } 
        public string? Username { get; set; }
        public TelegramUserStates State { get; set; }
        public string FirstName { get; set; }
        public Guid Key { get; set; }
        public string PhotoUrl { get; set; }
    }
}