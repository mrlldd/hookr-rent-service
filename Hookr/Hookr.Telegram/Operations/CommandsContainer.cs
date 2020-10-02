using System;
using System.Collections.Generic;
using System.Linq;
using Hookr.Telegram.Utilities.Extensions;
using Serilog;

namespace Hookr.Telegram.Operations
{
    public class CommandsContainer : ICommandsContainer
    {
        private readonly IDictionary<string, Type> nameToInterfaceType;

        public CommandsContainer()
        {
            nameToInterfaceType = typeof(CommandsContainer)
                .Assembly
                .ExtractCommandServicesTypes()
                .ToDictionary(
                    x => x.Implementation.Name
                        .ExtractCommandName(),
                    x => x.Interface);
            Log.Information("Collected {0} commands: {1}", nameToInterfaceType.Count, nameToInterfaceType.Keys.ToJson());
        }

        public Type? TryGetByCommandName(string? commandName)
        {
            nameToInterfaceType.TryGetValue(commandName ?? string.Empty, out var result);
            return result;
        }
    }
}