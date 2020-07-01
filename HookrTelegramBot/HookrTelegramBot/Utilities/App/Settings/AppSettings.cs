namespace HookrTelegramBot.Utilities.App.Settings
{
    public class AppSettings : IAppSettings
    {
        public AppSettings()
        {
            Database = new DatabaseConfig();
        }
        public string TelegramBotToken { get; set; }
        public string WebhookUrl { get; set; }
        public IDatabaseConfig Database { get; }
    }
}