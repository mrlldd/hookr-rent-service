using System.Linq;
using System.Reflection;
using Hookr.Core.Internal.Utilities.Extensions;
using Hookr.Core.Internal.Utilities.Loaders;
using Hookr.Core.Repository.Context.Entities.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Core.Utilities.Loaders
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoaders(this IServiceCollection services, Assembly assembly)
            => services
                .AddScoped<ILoaderProvider, LoaderProvider>()
                .WithLoaders(assembly);

        private static IServiceCollection WithLoaders(this IServiceCollection services, Assembly assembly)
            => services
                .AddScoped<ICachingLoader<int, TelegramUser>, TelegramUserLoader>()
                .LoadInterfacedImplementationsFromAssembly(
                    assembly,
                    typeof(CachingLoader<,>).GetGenericTypeDefinition(),
                    services.AddScoped,
                    types => types
                        .Where(x => x.GetGenericArguments().Length == 2)
                );
    }
}