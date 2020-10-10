using System;
using System.Collections.Generic;
using System.Linq;
using Hookr.Core.Utilities.Extensions;
using Hookr.Telegram.Utilities.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Hookr.Telegram.Operations
{
    public class CommandsContainer : ICommandsContainer
    {
        private readonly ILogger<CommandsContainer> logger;
        private readonly IDictionary<string, Type> nameToInterfaceType;

        public CommandsContainer(ILogger<CommandsContainer> logger)
        {
            this.logger = logger;
            nameToInterfaceType = typeof(CommandsContainer)
                .Assembly
                .ExtractCommandServicesTypes()
                .ToDictionary(
                    x => x.Implementation.Name
                        .ExtractCommandName(),
                    x => x.Interface);
            logger.LogInformation("Collected {0} commands: {@1}", nameToInterfaceType.Count, nameToInterfaceType.Keys);
        }

        public Type? TryGetByCommandName(string? commandName)
        {
            nameToInterfaceType.TryGetValue(commandName ?? string.Empty, out var result);
            return result;
        }
    }
}