using Microsoft.Extensions.DependencyInjection;

namespace HookrTelegramBot.Utilities.Telegram.Notifiers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNotifiers(this IServiceCollection services)
            => services.AddScoped<ITelegramUsersNotifier, TelegramUsersNotifier>();
    }
}