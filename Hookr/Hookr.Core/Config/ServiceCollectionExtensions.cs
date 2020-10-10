using Hookr.Core.Config.Telegram;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Core.Config
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, ICoreApplicationConfig config)
            => services
                .AddSingleton(config)
                .AddSingleton(config.Database)
                .AddSingleton(config.Telegram);
    }
}