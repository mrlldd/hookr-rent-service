using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Bot
{
    public interface ICurrentUpdateProvider
    {
        ExtendedUpdate Instance { get; }
        void Set(Update update);
    }
}