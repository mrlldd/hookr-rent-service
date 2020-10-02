using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Telegram.Bot;

namespace Hookr.Telegram.Utilities.Telegram.Bot.Client
{
    public interface IExtendedTelegramBotClient : ITelegramBotClient
    {
        ICurrentTelegramUserClient WithCurrentUser { get; }
    }
}