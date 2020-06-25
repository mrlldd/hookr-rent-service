using Microsoft.Extensions.DependencyInjection;

namespace HookrTelegramBot.Utilities.Selectors
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSelectors(this IServiceCollection services)
            => services
                .AddSingleton<IChatSelector, ChatSelector>();
    }
}