using System.Threading.Tasks;
using Telegram.Bot;

namespace HookrTelegramBot.Utilities.Telegram.Bot
{
    public interface ITelegramBotProvider
    {
        ITelegramBotClient Instance { get; }

        Task InitializeAsync();
    }
}