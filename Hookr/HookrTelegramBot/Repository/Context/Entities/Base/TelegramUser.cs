using System;

namespace HookrTelegramBot.Repository.Context.Entities.Base
{
    public class TelegramUser
    {
        // same as in telegram "database"
        public int Id { get; set; } 
        public string Username { get; set; }
        public TelegramUserStates State { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}