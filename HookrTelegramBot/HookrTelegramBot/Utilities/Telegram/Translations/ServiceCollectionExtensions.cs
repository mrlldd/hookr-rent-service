using Microsoft.Extensions.DependencyInjection;

namespace HookrTelegramBot.Utilities.Telegram.Translations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTranslations(this IServiceCollection services)
            => services
                .AddScoped<ITranslationsResolver, TranslationsResolver>();
    }
}