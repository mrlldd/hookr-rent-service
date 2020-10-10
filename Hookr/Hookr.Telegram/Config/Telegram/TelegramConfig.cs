using Hookr.Core.Config.Telegram;

namespace Hookr.Telegram.Config.Telegram
{
    public class TelegramConfig : Core.Config.Telegram.TelegramConfig, ITelegramConfig
    {
        public string Webhook { get; set; }
    }
}