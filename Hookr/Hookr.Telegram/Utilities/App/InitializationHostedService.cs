using System.Threading;
using System.Threading.Tasks;
using Hookr.Telegram.Utilities.Telegram.Bot.Provider;
using Microsoft.Extensions.Hosting;

namespace Hookr.Telegram.Utilities.App
{
    public class InitializationHostedService : IHostedService
    {
        private readonly ITelegramBotProvider telegramBotProvider;

        public InitializationHostedService(ITelegramBotProvider telegramBotProvider)
        {
            this.telegramBotProvider = telegramBotProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
            => telegramBotProvider.InitializeAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}