using System;

namespace Hookr.Telegram.Config.Management
{
    public interface IManagementConfig
    {
        Guid ServiceKey { get; }
        Guid DeveloperKey { get; }
    }
}