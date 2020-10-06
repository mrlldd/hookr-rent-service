using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Core.Repository
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
            => services
                .AddScoped<IHookrRepository, HookrRepository>();
    }
}