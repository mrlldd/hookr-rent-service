using System;

namespace HookrTelegramBot.Utilities.App.Settings
{
    public class AdminConfig : IAdminConfig
    {
        public Guid ServiceKey { get; set; }
        public Guid DeveloperKey { get; set; }
    }
}