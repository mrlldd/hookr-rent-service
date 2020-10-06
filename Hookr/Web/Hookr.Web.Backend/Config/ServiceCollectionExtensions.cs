using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Web.Backend.Config
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, IApplicationConfig config)
            => services
                .AddSingleton(config)
                .AddSingleton(config.Database)
                .AddSingleton(config.Telegram);
    }
}