using System.Reflection;
using Hookr.Core.Config.Cache;
using Hookr.Core.Internal.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Hookr.Core.Utilities.Caches
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaches(this IServiceCollection services, Assembly assembly,
            ICacheConfig config)
            => services
                .AddMemoryCache()
                .AddScoped<ICacheProvider, CacheProvider>()
                .WithCaches(assembly)
                .AddStackExchangeRedisCache(x =>
                {
                    var options = x.ConfigurationOptions = ConfigurationOptions.Parse(config.ConnectionString);
                    options.ReconnectRetryPolicy = new LinearRetry(config.LinearRetries);
                    options.KeepAlive = config.KeepAliveSeconds;
                });

        private static IServiceCollection WithCaches(this IServiceCollection services, Assembly assembly)
            => services
                .LoadInterfacedImplementationsFromAssembly(
                    assembly,
                    typeof(Cache<>).GetGenericTypeDefinition(),
                    services.AddScoped
                );
    }
}