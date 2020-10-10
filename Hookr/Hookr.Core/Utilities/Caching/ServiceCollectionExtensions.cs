using System.Reflection;
using Hookr.Core.Internal.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Core.Utilities.Caching
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaches(this IServiceCollection services, Assembly assembly)
            => services
                .WithCaches(assembly);

        private static IServiceCollection WithCaches(this IServiceCollection services, Assembly assembly)
            => services
                .LoadInterfacedImplementationsFromAssembly(
                    assembly,
                    typeof(ICache<>),
                    services.AddScoped
                );
    }
}