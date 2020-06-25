using System;
using System.Linq;
using System.Reflection;
using HookrTelegramBot.Operations.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HookrTelegramBot.Operations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramHandlers(this IServiceCollection services)
        {
            var commandType = typeof(Command<>);
            return typeof(ServiceCollectionExtensions)
                .Assembly
                .GetTypes()
                .Where(x => x != commandType && !x.IsAbstract && x.IsAssignableFrom(commandType))
                .Select<Type, (Type Interface, Type Implementation)>(x => (x.GetInterfaces().First(), x))
                .Aggregate(services,
                    (collection, next) => collection
                        .AddScoped(next.Interface, next.Implementation));
        }
    }
}