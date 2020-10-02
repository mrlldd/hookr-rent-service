using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Telegram.Utilities.App
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLevelServices(this IServiceCollection services)
            => services
                .AddHostedService<InitializationHostedService>();
    }
}