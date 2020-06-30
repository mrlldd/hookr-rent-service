using System.Threading.Tasks;
using Telegram.Bot;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Provider
{
    public interface ITelegramBotProvider
    {
        ITelegramBotClient Instance { get; }

        Task InitializeAsync();
    }
}