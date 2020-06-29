using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Selectors
{
    public interface IChatSelector
    {
        Chat Select<TUpdate>(TUpdate update) where TUpdate : ExtendedUpdate;
    }
}