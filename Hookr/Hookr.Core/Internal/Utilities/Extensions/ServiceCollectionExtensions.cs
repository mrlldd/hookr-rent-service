using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hookr.Core.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Hookr.Core.Internal.Utilities.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadInterfacedImplementationsFromAssembly<T>(this IServiceCollection services,
            Assembly assembly,
            Func<Type, Type, IServiceCollection> adder,
            Func<IEnumerable<Type>, IEnumerable<Type>>? customQuery = null)
            => LoadInterfacedImplementationsFromAssembly(services, assembly, typeof(T), adder, customQuery);

        public static IServiceCollection LoadInterfacedImplementationsFromAssembly(this IServiceCollection services,
            Assembly assembly,
            Type type,
            Func<Type, Type, IServiceCollection> adder,
            Func<IEnumerable<Type>, IEnumerable<Type>>? customQuery = null)
        {
            var interfaces = type
                .GetInterfaces()
                .Select(DefinitionIfGeneric);
            return GetTypesFromAssembly(assembly, type, customQuery ?? (x => x))
                .Aggregate(services,
                    (prev, next) => adder(next
                            .GetInterfaces()
                            .First(x => !interfaces.Contains(DefinitionIfGeneric(x))),
                        next)
                );
        }

        public static IServiceCollection LoadImplementationsFromAssembly<T>(this IServiceCollection services,
            Assembly assembly,
            Func<Type, IServiceCollection> adder,
            Func<IEnumerable<Type>, IEnumerable<Type>>? customQuery = null)
            => LoadImplementationsFromAssembly(services, assembly, typeof(T), adder, customQuery);

        public static IServiceCollection LoadImplementationsFromAssembly(this IServiceCollection services,
            Assembly assembly,
            Type type,
            Func<Type, IServiceCollection> adder,
            Func<IEnumerable<Type>, IEnumerable<Type>>? customQuery = null)
            => GetTypesFromAssembly(assembly, type, customQuery ?? (x => x))
                .Aggregate(services, (prev, next) => adder(next));

        private static IEnumerable<Type> GetTypesFromAssembly(Assembly assembly,
            Type type,
            Func<IEnumerable<Type>, IEnumerable<Type>> customQuery)
            => assembly
                .GetTypes()
                .Where(type.IsGenericType
                    ? (Func<Type, bool>) (x => IsSubclassOfRawGeneric(type, x))
                    : type.IsAssignableFrom)
                .Where(x => !x.IsAbstract)
                .Map(customQuery);

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
            => toCheck != null && toCheck != typeof(object) &&
               (generic == DefinitionIfGeneric(toCheck)
                || IsSubclassOfRawGeneric(generic, toCheck.BaseType));

        private static Type DefinitionIfGeneric(Type type) =>
            type.IsGenericType
                ? type.GetGenericTypeDefinition()
                : type;
    }
}