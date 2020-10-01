using System;
using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Selectors.UpdateMessage
{
    public interface IUpdateMessageSelectorsContainer
    {
        Func<Update, Message> GetSelector(Update update);
    }
}