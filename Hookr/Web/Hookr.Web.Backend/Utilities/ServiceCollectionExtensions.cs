using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Web.Backend.Utilities
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUtilities(this IServiceCollection services)
            => services
                .AddScoped<IUserContextAccessor, UserContextAccessor>();
    }
}