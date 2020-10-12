using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Caches;
using Hookr.Telegram.Utilities.Telegram.Notifiers;
using Hookr.Telegram.Utilities.Telegram.Selectors;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Telegram.Utilities.Telegram
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramServices(this IServiceCollection services)
            => services
                .AddTelegramSelectors()
                .AddTelegramBotServices()
                .AddNotifiers()
                .AddTranslations();

    }
}