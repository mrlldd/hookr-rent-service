using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace HookrTelegramBot.Utilities.Telegram.Bot
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramBotServices(this IServiceCollection services)
            => services
                .AddSingleton<ITelegramBotProvider, TelegramBotProvider>()
                .AddScoped<IExtendedTelegramBotClient, ExtendedTelegramBotClient>();
    }
}