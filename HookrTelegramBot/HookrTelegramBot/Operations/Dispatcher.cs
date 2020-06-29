using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HookrTelegramBot.Operations
{
    public class Dispatcher : IDispatcher
    {
        private readonly ICommandsContainer commandsContainer;

        public Dispatcher(ICommandsContainer commandsContainer)
        {
            this.commandsContainer = commandsContainer;
        }


        public Task DispatchAsync(Update update)
        {
            throw new NotImplementedException();
        }
    }
}