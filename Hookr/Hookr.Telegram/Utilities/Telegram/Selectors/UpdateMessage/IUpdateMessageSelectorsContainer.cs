using System;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Utilities.Telegram.Selectors.UpdateMessage
{
    public interface IUpdateMessageSelectorsContainer
    {
        Func<Update, Message> GetSelector(Update update);
    }
}