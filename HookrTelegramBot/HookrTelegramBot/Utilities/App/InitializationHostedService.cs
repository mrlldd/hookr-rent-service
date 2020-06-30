using System.Threading;
using System.Threading.Tasks;
using HookrTelegramBot.Utilities.Telegram.Bot.Provider;
using Microsoft.Extensions.Hosting;

namespace HookrTelegramBot.Utilities.App
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