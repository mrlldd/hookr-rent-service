using Hookr.Core.Config.Telegram;

namespace Hookr.Telegram.Config.Telegram
{
    public class ExtendedTelegramConfig : TelegramConfig, IExtendedTelegramConfig
    {
        public string Webhook { get; set; }
    }
}