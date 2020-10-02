using Hookr.Telegram.Utilities.Telegram.Caches.CurrentOrder;
using Hookr.Telegram.Utilities.Telegram.Caches.UserTemporaryStatus;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Telegram.Utilities.Telegram.Caches
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaches(this IServiceCollection services)
            => services
                .AddSingleton<IUserTemporaryStatusCache, UserTemporaryStatusCache>()
                .AddSingleton<ICurrentOrderCache, CurrentOrderCache>();
    }
}