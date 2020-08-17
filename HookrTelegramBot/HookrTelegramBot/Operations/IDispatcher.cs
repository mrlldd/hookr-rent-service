
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations
{
    public interface IDispatcher
    {
        Task DispatchAsync();
    }
}