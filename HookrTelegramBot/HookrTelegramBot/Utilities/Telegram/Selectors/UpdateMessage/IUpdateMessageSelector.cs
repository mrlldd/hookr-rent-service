using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Selectors.UpdateMessage
{
    public interface IUpdateMessageSelector
    {
        Message Select(ExtendedUpdate update);
    }
}