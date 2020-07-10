using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Get;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Caches;
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
        private readonly IUserTemporaryStatusCache userTemporaryStatusCache;
        private readonly IDictionary<UserTemporaryStatus, Func<Task>> userResponseHandlers;

        public Dispatcher(ICommandsContainer commandsContainer,
            IServiceProvider serviceProvider,
            IUserContextProvider userContextProvider,
            IUserTemporaryStatusCache userTemporaryStatusCache)
        {
            this.commandsContainer = commandsContainer;
            this.serviceProvider = serviceProvider;
            this.userContextProvider = userContextProvider;
            this.userTemporaryStatusCache = userTemporaryStatusCache;

            userResponseHandlers = new Dictionary<UserTemporaryStatus, Func<Task>>
            {
                {UserTemporaryStatus.Default, ThrowDispatchingExceptionAsync},
                {UserTemporaryStatus.WaitingForHookah, GetDetailedHookahInfo}
            };
        }


        public Task DispatchAsync()
        {
            var update = userContextProvider.Update;
            var data = update.Type == UpdateType.CallbackQuery
                ? update.CallbackQuery.Data
                : update.RealMessage.Text;
            var commandName = data.ExtractCommand();
            if (commandName.IsNumber())
            {
                var userStatus = userTemporaryStatusCache.Get(update.RealMessage.From.Id);
                Log.Information("Dispatching user with status {0} response {1}", userStatus, commandName);
                return userResponseHandlers[userStatus]();
            }

            return DispatchAndExecuteCommandAsync(commandName);
        }

        private Task DispatchAndExecuteCommandAsync(string commandName)
        {
            Log.Information("Dispatching command {0}", commandName);
            var commandType = commandsContainer.TryGetByCommandName(commandName);
            if (commandType == null)
            {
                return ThrowDispatchingExceptionAsync();
            }

            var service = (ICommand) serviceProvider.GetService(commandType);
            return service == null
                ? ThrowDispatchingExceptionAsync()
                : service.ExecuteAsync();
        }

        private Task GetDetailedHookahInfo()
            => DispatchAndExecuteCommandAsync(nameof(GetHookahCommand).ExtractCommandName());

        private Task ThrowDispatchingExceptionAsync()
            => throw new InvalidOperationException("There is no such command :(");
    }
}