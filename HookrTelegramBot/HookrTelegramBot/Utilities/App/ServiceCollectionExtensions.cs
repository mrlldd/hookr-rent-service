using Microsoft.Extensions.DependencyInjection;

namespace HookrTelegramBot.Utilities.App
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLevelServices(this IServiceCollection services)
            => services;
    }
}