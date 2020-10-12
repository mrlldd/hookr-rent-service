using System.Linq;
using System.Reflection;
using Hookr.Web.Backend.Operations.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Web.Backend.Operations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOperations(this IServiceCollection services)
            => services
                .AddScoped<Dispatcher>()
                .AddCommandsAndQueries();

        private static IServiceCollection AddCommandsAndQueries(this IServiceCollection services)
            => Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(typeof(Handler).IsAssignableFrom)
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetInterfaces().Any())
                .Aggregate(services, (prev, next) => prev
                    .AddScoped(next
                            .GetInterfaces()
                            .First(),
                        next)
                );
    }
}