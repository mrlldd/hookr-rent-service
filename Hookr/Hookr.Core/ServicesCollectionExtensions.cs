using Hookr.Core.Utilities.Loaders;
using Hookr.Core.Utilities.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Core
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddHookrCore<TTelegramUserIdProvider>(this IServiceCollection services) 
            where TTelegramUserIdProvider : class, ITelegramUserIdProvider
            => services
                .AddScoped<ITelegramUserIdProvider, TTelegramUserIdProvider>()
                .AddLoaders();
    }
}