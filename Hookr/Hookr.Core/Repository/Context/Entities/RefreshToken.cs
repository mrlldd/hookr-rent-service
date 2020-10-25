using System;
using Hookr.Core.Repository.Context.Entities.Base;

namespace Hookr.Core.Repository.Context.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public Guid Value { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool Used { get; set; }
        
        public int UserId { get; set; }
        
        public TelegramUser User { get; set; }
    }
}