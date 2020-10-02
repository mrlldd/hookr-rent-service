using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hookr.Telegram.Operations.Base;

namespace Hookr.Telegram.Utilities.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<(Type Interface, Type Implementation)> ExtractCommandServicesTypes(this Assembly assembly)
        {
            var commandType = typeof(Command);
            return assembly
                .GetTypes()
                .Where(x => x != commandType && !x.IsAbstract && commandType.IsAssignableFrom(x))
                .Select(x => (x.GetInterfaces().FirstOrDefault(y => y != commandType && y != typeof(ICommand)), x));
        }
    }
}