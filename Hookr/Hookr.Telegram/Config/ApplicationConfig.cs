using Hookr.Core.Config;
using Hookr.Telegram.Config.Management;

namespace Hookr.Telegram.Config
{
    public class ApplicationConfig : CoreApplicationConfig, IApplicationConfig
    {
        public ApplicationConfig()
        {
            Management = new ManagementConfig();
        }
        public IManagementConfig Management { get; }
    }
}