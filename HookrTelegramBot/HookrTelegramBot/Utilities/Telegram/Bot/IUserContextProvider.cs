using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Bot
{
    public interface IUserContextProvider
    {
        ExtendedUpdate Update { get; }
        Message Message { get; }
        void Set(Update update);
    }
}