using Hookr.Core.Config.Telegram;

namespace Hookr.Telegram.Config.Telegram
{
    public interface ITelegramConfig : Core.Config.Telegram.ITelegramConfig
    {
        string Webhook { get; }
    }
}