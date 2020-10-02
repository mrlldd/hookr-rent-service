using Hookr.Telegram.Models.Telegram;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Utilities.Telegram.Bot.Factory
{
    public interface IExtendedUpdateFactory
    {
        ExtendedUpdate Create(Update update);
    }
}