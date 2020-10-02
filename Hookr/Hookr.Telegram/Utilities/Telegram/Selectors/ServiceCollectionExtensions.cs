using Hookr.Telegram.Utilities.Telegram.Selectors.UpdateMessage;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Telegram.Utilities.Telegram.Selectors
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramSelectors(this IServiceCollection services)
            => services
                .AddSingleton<IUpdateMessageSelectorsContainer, UpdateMessageSelectorsContainer>();
    }
}