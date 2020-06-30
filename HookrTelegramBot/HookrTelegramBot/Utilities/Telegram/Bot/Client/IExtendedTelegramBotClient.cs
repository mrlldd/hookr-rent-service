using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Client
{
    public interface IExtendedTelegramBotClient : ITelegramBotClient
    {
        Task<Message> SendTextMessageToCurrentUserAsync(string text);
    }
}