using System;
using System.Collections.Generic;
using System.Linq;
using HookrTelegramBot.Utilities.Extensions;
using Serilog;

namespace HookrTelegramBot.Operations
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
            Log.Information("Collected {0} commands.", nameToInterfaceType.Count);
        }

        public Type TryGetByCommandName(string commandName)
        {
            nameToInterfaceType.TryGetValue(commandName, out var result);
            return result;
        }
    }
}