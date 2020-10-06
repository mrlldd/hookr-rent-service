namespace Hookr.Telegram.Config.Management
{
    public class ManagementConfig : IManagementConfig
    {
        public string ServiceKey { get; set; }
        public string DeveloperKey { get; set; }
    }
}