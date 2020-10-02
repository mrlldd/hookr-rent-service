using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hookr.Telegram.Operations.Base;
using Hookr.Telegram.Operations.Commands.Administration.Hookahs.Get;
using Hookr.Telegram.Operations.Commands.Administration.Hookahs.Photos;
using Hookr.Telegram.Operations.Commands.Administration.Tobaccos.Get;
using Hookr.Telegram.Operations.Commands.Administration.Tobaccos.Photos;
using Hookr.Telegram.Utilities.Extensions;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Caches.UserTemporaryStatus;
using Serilog;

namespace Hookr.Telegram.Operations
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
                {UserTemporaryStatus.Default, ThrowCommandExecutingExceptionAsync},
                {UserTemporaryStatus.ChoosingHookah, ExecuteCommandAsync<GetHookahCommand>},
                {UserTemporaryStatus.ChoosingTobacco, ExecuteCommandAsync<GetTobaccoCommand>},
                {UserTemporaryStatus.AskedForTobaccoPhotos, ExecuteCommandAsync<SetTobaccoPhotosCommand>},
                {UserTemporaryStatus.AskedForHookahPhotos, ExecuteCommandAsync<SetHookahPhotosCommand>}
            };
        }


        public Task DispatchAsync()
        {
            var update = userContextProvider.Update;
            var userStatus = userTemporaryStatusCache.Get(update.RealMessage.From.Id);
            var data = update.Content;
            if (string.IsNullOrEmpty(data) || data.ExtractCommand().IsNumber())
            {
                if (userStatus != UserTemporaryStatus.Default
                    && userResponseHandlers.TryGetValue(userStatus, out var handler))
                {
                    Log.Information("Dispatching user with status {0} response {1}", userStatus, data);
                    return handler();
                }
            }

            return ExecuteCommandAsync(data.ExtractCommand());
        }

        private Task ExecuteCommandAsync(string? commandName)
        {
            Log.Information("Dispatching command {0}", commandName);
            var commandType = commandsContainer.TryGetByCommandName(commandName);
            return commandType != null && serviceProvider.GetService(commandType) is ICommand command
                ? command.ExecuteAsync()
                : ThrowCommandExecutingExceptionAsync(); 
            /*if (commandType == null)
            {
                return ThrowCommandExecutingExceptionAsync();
            }

            return serviceProvider.GetService(commandType) is ICommand command
                ? command.ExecuteAsync()
                : ThrowCommandExecutingExceptionAsync();*/
        }

        private Task ExecuteCommandAsync<TCommand>() where TCommand : Command
            => ExecuteCommandAsync(typeof(TCommand).Name.ExtractCommandName());

        private static Task ThrowCommandExecutingExceptionAsync()
            => throw new InvalidOperationException("There is no such command :(");
    }
}