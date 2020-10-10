using System;
using System.Linq;
using System.Reflection;
using Hookr.Core.Internal.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Core.Utilities.Loaders
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoaders(this IServiceCollection services, Assembly assembly)
            => services
                .AddScoped<CachingLoaderDispatcher>()
                .WithLoaders(assembly);

        private static IServiceCollection WithLoaders(this IServiceCollection services, Assembly assembly)
            => services
                    .LoadImplementationsFromAssembly(
                        assembly,
                        typeof(CachingLoader<,>),
                        services.AddScoped,
                        types => types
                            .Where(x => x.GetGenericArguments().Length == 2)
                    );
    }
}