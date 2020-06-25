using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Selectors
{
    public interface IChatSelector
    {
        Chat Select<TUpdate>(TUpdate update) where TUpdate : ExtendedUpdate;
    }
}