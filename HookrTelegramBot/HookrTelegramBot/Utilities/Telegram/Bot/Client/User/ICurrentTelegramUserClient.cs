using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Client.User
{
    public interface ICurrentTelegramUserClient
    {
        Task<Message> SendTextMessageAsync(string text);
    }
}