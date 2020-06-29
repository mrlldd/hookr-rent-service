
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations
{
    public interface IDispatcher
    {
        Task DispatchAsync(Update update);
    }
}