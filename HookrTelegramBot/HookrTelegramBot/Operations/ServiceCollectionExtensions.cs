using System;
using System.Linq;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace HookrTelegramBot.Operations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOperations(this IServiceCollection services)
        {
            return typeof(ServiceCollectionExtensions)
                .Assembly
                .ExtractCommandServicesTypes()
                .Aggregate(services,
                    (collection, next) => collection
                        .AddScoped(next.Interface, next.Implementation))
                .AddSingleton<ICommandsContainer, CommandsContainer>()
                .AddScoped<IDispatcher, Dispatcher>();
        }
    }
}