using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Factory
{
    public interface IExtendedUpdateFactory
    {
        ExtendedUpdate Create(Update update);
    }
}