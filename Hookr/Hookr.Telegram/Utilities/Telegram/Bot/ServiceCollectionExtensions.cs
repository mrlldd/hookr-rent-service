using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Factory;
using Hookr.Telegram.Utilities.Telegram.Bot.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Telegram.Utilities.Telegram.Bot
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramBotServices(this IServiceCollection services)
            => services
                .AddSingleton<ITelegramBotProvider, TelegramBotProvider>()
                .AddSingleton<IExtendedUpdateFactory, ExtendedUpdateFactory>()
                .AddScoped<IExtendedTelegramBotClient, ExtendedTelegramBotClient>()
                .AddScoped<IUserContextProvider, UserContextProvider>();
    }
}