using System.Reflection;
using Hookr.Core.Repository;
using Hookr.Core.Utilities.Caches;
using Hookr.Core.Utilities.Loaders;
using Hookr.Core.Utilities.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Core
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddHookrCore(this IServiceCollection services, Assembly loadTypesFrom)
            => services
                .AddScoped<ITelegramUserIdProvider, TelegramUserIdProvider>()
                .AddScoped<IHookrRepository, HookrRepository>()
                .AddLoaders(loadTypesFrom)
                .AddCaches(loadTypesFrom);
    }
}