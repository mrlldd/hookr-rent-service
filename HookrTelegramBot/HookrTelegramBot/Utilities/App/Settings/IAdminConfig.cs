using System;

namespace HookrTelegramBot.Utilities.App.Settings
{
    public interface IAdminConfig
    {
        Guid ServiceKey { get; }
        Guid DeveloperKey { get; }
    }
}