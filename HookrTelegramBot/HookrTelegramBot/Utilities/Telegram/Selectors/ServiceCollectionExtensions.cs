using Microsoft.Extensions.DependencyInjection;

namespace HookrTelegramBot.Utilities.Telegram.Selectors
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramSelectors(this IServiceCollection services)
            => services
                .AddSingleton<IChatSelector, ChatSelector>();
    }
}