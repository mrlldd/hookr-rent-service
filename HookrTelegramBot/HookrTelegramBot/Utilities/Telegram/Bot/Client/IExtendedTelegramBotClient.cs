using HookrTelegramBot.Utilities.Telegram.Bot.Client.User;
using Telegram.Bot;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Client
{
    public interface IExtendedTelegramBotClient : ITelegramBotClient
    {
        ICurrentTelegramUserClient WithCurrentUser { get; }
    }
}