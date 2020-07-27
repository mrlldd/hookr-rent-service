using HookrTelegramBot.Utilities.Telegram.Caches.CurrentOrder;
using HookrTelegramBot.Utilities.Telegram.Caches.UserTemporaryStatus;
using Microsoft.Extensions.DependencyInjection;

namespace HookrTelegramBot.Utilities.Telegram.Caches
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaches(this IServiceCollection services)
            => services
                .AddSingleton<IUserTemporaryStatusCache, UserTemporaryStatusCache>()
                .AddSingleton<ICurrentOrderCache, CurrentOrderCache>();
    }
}