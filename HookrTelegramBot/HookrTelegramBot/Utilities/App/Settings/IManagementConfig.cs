using System;

namespace HookrTelegramBot.Utilities.App.Settings
{
    public interface IManagementConfig
    {
        Guid ServiceKey { get; }
        Guid DeveloperKey { get; }
    }
}