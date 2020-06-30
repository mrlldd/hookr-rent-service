using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Base;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HookrTelegramBot.Operations
{
    public class Dispatcher : IDispatcher
    {
        private readonly ICommandsContainer commandsContainer;
        private readonly IServiceProvider serviceProvider;

        public Dispatcher(ICommandsContainer commandsContainer, IServiceProvider serviceProvider)
        {
            this.commandsContainer = commandsContainer;
            this.serviceProvider = serviceProvider;
        }


        public Task DispatchAsync(Update update)
        {
            var service = (ICommand) serviceProvider.GetService(commandsContainer.TryGetByCommandName("start"));
            return service.ExecuteAsync();
        }
    }
}