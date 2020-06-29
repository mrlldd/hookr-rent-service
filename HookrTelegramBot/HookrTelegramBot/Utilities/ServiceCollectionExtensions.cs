using HookrTelegramBot.Utilities.App;
using HookrTelegramBot.Utilities.Telegram;
using Microsoft.Extensions.DependencyInjection;

namespace HookrTelegramBot.Utilities
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUtilities(this IServiceCollection services)
            => services
                .AddApplicationLevelServices()
                .AddTelegramServices();
    }
}