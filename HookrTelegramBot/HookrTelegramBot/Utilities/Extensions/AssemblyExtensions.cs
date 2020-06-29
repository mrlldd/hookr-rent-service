using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HookrTelegramBot.Operations.Base;

namespace HookrTelegramBot.Utilities.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<(Type Interface, Type Implementation)> ExtractCommandServicesTypes(this Assembly assembly)
        {
            var commandType = typeof(ICommand);
            return assembly
                .GetTypes()
                .Where(x => x != commandType && !x.IsAbstract && commandType.IsAssignableFrom(x))
                .Select<Type, (Type Interface, Type Implementation)>(x => (x.GetInterfaces().FirstOrDefault(y => y != commandType), x));
        }
    }
}