using Hookr.Core.Config;
using Hookr.Telegram.Config.Management;

namespace Hookr.Telegram.Config
{
    public interface IApplicationConfig : ICoreApplicationConfig
    {
        IManagementConfig Management { get; }
    }
}