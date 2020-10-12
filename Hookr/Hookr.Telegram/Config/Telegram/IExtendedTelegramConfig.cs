namespace Hookr.Telegram.Config.Telegram
{
    public interface IExtendedTelegramConfig : Core.Config.Telegram.ITelegramConfig
    {
        string Webhook { get; }
    }
}