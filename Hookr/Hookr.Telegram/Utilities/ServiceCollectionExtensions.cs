using Hookr.Core.Utilities.Resiliency;
using Hookr.Telegram.Utilities.App;
using Hookr.Telegram.Utilities.Telegram;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Telegram.Utilities
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUtilities(this IServiceCollection services)
            => services
                .AddApplicationLevelServices()
                .AddTelegramServices()
                .AddSingleton<IPolicySet, PolicySet>();
    }
}