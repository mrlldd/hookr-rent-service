using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Core.Config
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfig<T>(this IServiceCollection services, T config) where T : class, ICoreApplicationConfig
            => services
                .AddSingleton(config)
                .AddSingleton(config.Database)
                .AddSingleton(config.Telegram);
    }
}