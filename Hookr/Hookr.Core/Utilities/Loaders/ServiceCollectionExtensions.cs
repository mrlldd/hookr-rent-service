using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Core.Utilities.Loaders
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoaders(this IServiceCollection services)
            => services
                .AddScoped<CachingLoaderDispatcher>()
                .WithLoaders();

        private static IServiceCollection WithLoaders(this IServiceCollection services) 
            => Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(typeof(ILoader).IsAssignableFrom)
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetGenericArguments().Length == 2)
                .Aggregate(services, (prev, next) => prev.AddScoped(next));
    }
}