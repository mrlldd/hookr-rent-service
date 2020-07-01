using Microsoft.Extensions.DependencyInjection;

namespace HookrTelegramBot.Repository
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
            => services
                .AddScoped<IHookrRepository, HookrRepository>();
    }
}