using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Get;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Get;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Photos;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Caches;
using HookrTelegramBot.Utilities.Telegram.Caches.UserTemporaryStatus;
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
                {UserTemporaryStatus.ChoosingHookah, GetDetailedHookahInfo},
                {UserTemporaryStatus.ChoosingTobacco, GetDetailedTobaccoInfo},
                {UserTemporaryStatus.AskedForTobaccoPhotos, SetTobaccoPhotos}
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

            return DispatchAndExecuteCommandAsync(data.ExtractCommand());
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

        private Task GetDetailedTobaccoInfo()
            => DispatchAndExecuteCommandAsync(nameof(GetTobaccoCommand).ExtractCommandName());

        private Task SetTobaccoPhotos()
            => DispatchAndExecuteCommandAsync(nameof(SetTobaccoPhotosCommand).ExtractCommandName());

        private static Task ThrowDispatchingExceptionAsync()
            => throw new InvalidOperationException("There is no such command :(");
    }
}