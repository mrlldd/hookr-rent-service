using System;

namespace HookrTelegramBot.Utilities.App.Settings
{
    public class ManagementConfig : IManagementConfig
    {
        public Guid ServiceKey { get; set; }
        public Guid DeveloperKey { get; set; }
    }
}