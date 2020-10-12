using System;
using System.Linq;
using System.Reflection;
using Hookr.Core.Internal.Utilities.Extensions;
using Hookr.Core.Utilities.Caching;
using Hookr.Core.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hookr.Core.Utilities.Caches
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaches(this IServiceCollection services, Assembly assembly)
            => services
                .AddMemoryCache()
                .AddDistributedMemoryCache()
                .AddScoped<ICacheProvider, CacheProvider>()
                .WithCaches(assembly);

        private static IServiceCollection WithCaches(this IServiceCollection services, Assembly assembly)
            => services
                .LoadInterfacedImplementationsFromAssembly(
                    assembly,
                    typeof(Cache<>).GetGenericTypeDefinition(),
                    services.AddScoped
                );
    }
}