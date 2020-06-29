using Microsoft.Extensions.DependencyInjection;

namespace HookrTelegramBot.Utilities.Telegram.Bot
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramBotServices(this IServiceCollection services)
            => services
                .AddSingleton<ITelegramBotProvider, TelegramBotProvider>();
    }
}