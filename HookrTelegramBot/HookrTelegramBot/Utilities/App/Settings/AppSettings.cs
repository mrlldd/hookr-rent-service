namespace HookrTelegramBot.Utilities.App.Settings
{
    public class AppSettings : IAppSettings
    {
        public AppSettings()
        {
            Database = new DatabaseConfig();
            Admin = new AdminConfig();
        }
        public string TelegramBotToken { get; set; }
        public string WebhookUrl { get; set; }
        public IDatabaseConfig Database { get; }
        public IAdminConfig Admin { get; }
    }
}