using Hookr.Core.Config;
using Hookr.Telegram.Config.Management;
using Hookr.Telegram.Config.Telegram;

namespace Hookr.Telegram.Config
{
    public interface IApplicationConfig : ICoreApplicationConfig
    {
        IManagementConfig Management { get; }
    }
}