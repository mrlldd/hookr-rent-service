using System;
using System.Diagnostics.CodeAnalysis;

namespace Hookr.Telegram.Repository.Context.Entities.Base
{
    public class TelegramUser
    {
        // same as in telegram "database"
        public int Id { get; set; } 
        [NotNull]
        public string? Username { get; set; }
        public TelegramUserStates State { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}