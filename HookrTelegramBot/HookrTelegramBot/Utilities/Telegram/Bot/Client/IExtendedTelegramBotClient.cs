using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Telegram.Bot;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Client
{
    public interface IExtendedTelegramBotClient : ITelegramBotClient
    {
        ICurrentTelegramUserClient WithCurrentUser { get; }
    }
}