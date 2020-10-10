using System;

namespace Hookr.Telegram.Config.Management
{
    public class ManagementConfig : IManagementConfig
    {
        public Guid ServiceKey { get; set; }
        public Guid DeveloperKey { get; set; }
    }
}