namespace HookrTelegramBot.Utilities.App.Settings
{
    public class AppSettings : IAppSettings
    {
        public string TelegramBotToken { get; set; }
        public string WebhookUrl { get; set; }
    }
}