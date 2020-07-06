using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Bot
{
    public interface IUserContextProvider
    {
        ExtendedUpdate Update { get; }
        void Set(Update update);
    }
}