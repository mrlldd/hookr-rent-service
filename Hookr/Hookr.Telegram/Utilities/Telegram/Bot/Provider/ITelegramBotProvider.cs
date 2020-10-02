using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Hookr.Telegram.Utilities.Telegram.Bot.Provider
{
    public interface ITelegramBotProvider
    {
        ITelegramBotClient Instance { get; }

        Task InitializeAsync(CancellationToken token = default);
    }
}