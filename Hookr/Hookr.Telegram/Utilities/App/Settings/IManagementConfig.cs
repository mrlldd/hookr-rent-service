using System;

namespace Hookr.Telegram.Utilities.App.Settings
{
    public interface IManagementConfig
    {
        Guid ServiceKey { get; }
        Guid DeveloperKey { get; }
    }
}