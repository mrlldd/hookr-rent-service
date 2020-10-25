using System.Reflection;
using Hookr.Core.Config;
using Hookr.Core.Config.Cache;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context;
using Hookr.Core.Utilities.Caches;
using Hookr.Core.Utilities.Loaders;
using Hookr.Core.Utilities.Providers;
using Hookr.Core.Utilities.Resiliency;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Core
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddHookrCore(this IServiceCollection services, Assembly loadTypesFrom,
            ICoreApplicationConfig config)
            => services
                .AddAssemblyDependentServices(loadTypesFrom, config.Cache)
                .AddScoped<ITelegramUserIdProvider, TelegramUserIdProvider>()
                .AddScoped<IHookrRepository, HookrRepository>()
                .AddSingleton<IPolicySet, PolicySet>()
                .AddDbContext<HookrContext>(builder => builder
                        .UseHookrCoreConfig(config.Database)
#if DEBUG
                        .EnableSensitiveDataLogging()
#endif
                );

        private static IServiceCollection AddAssemblyDependentServices(this IServiceCollection services,
            Assembly assembly,
            ICacheConfig cacheConfig)
            => services
                .AddLoaders(assembly)
                .AddCaches(assembly, cacheConfig);
    }
}