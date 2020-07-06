using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using Serilog;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HookrTelegramBot.Operations
{
    public class Dispatcher : IDispatcher
    {
        private readonly ICommandsContainer commandsContainer;
        private readonly IServiceProvider serviceProvider;
        private readonly IUserContextProvider userContextProvider;

        public Dispatcher(ICommandsContainer commandsContainer,
            IServiceProvider serviceProvider,
            IUserContextProvider userContextProvider)
        {
            this.commandsContainer = commandsContainer;
            this.serviceProvider = serviceProvider;
            this.userContextProvider = userContextProvider;
        }


        public Task DispatchAsync()
        {
            var commandName = userContextProvider.Update.Message.Text.ExtractCommand();
            Log.Information("Dispatching command {0}", commandName);
            var service = (ICommand) serviceProvider.GetService(commandsContainer.TryGetByCommandName(commandName));
            if (service == null)
            {
                throw new InvalidOperationException("There is no such command :(");
            }
            return service.ExecuteAsync();
        }
    }
}