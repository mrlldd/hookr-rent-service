using System;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram.Exceptions;
using HookrTelegramBot.Models.Telegram.Exceptions.Base;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Serilog;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Base
{

    public abstract class Command : ICommand
    {
        protected readonly IExtendedTelegramBotClient TelegramBotClient;

        protected Command(IExtendedTelegramBotClient telegramBotClient)
        {
            TelegramBotClient = telegramBotClient;
        }

        public abstract Task ExecuteAsync();

        protected virtual Task<Message> SendErrorAsync(ICurrentTelegramUserClient client, Exception exception)
        {
            Log.Information(exception.ToString());
            var message = "Not available at the moment, sorry :(";
            if (exception is TelegramException)
            {
                switch (exception)
                {
                    case InsufficientAccessRightsException rightsException:
                    {
                        message = rightsException.Message;
                        break;
                    }
                    case InvalidArgumentsPassedInException passedInException:
                    {
                        message = passedInException.Message;
                        break;
                    }
                    default:
                    {
                        var (success, anotherMessage) = HandleCustomException(exception);
                        if (success)
                        {
                            message = anotherMessage;
                        }
                        break;
                    }
                }
            }
            return client.SendTextMessageAsync(message);
        }

        protected virtual (bool, string) HandleCustomException(Exception exception) => (false, string.Empty);
    }
    public abstract class CommandWithResponse : Command
    {
        protected CommandWithResponse(IExtendedTelegramBotClient telegramBotClient) : base(telegramBotClient)
        {
        }

        public override Task ExecuteAsync()
            => ProcessAsync()
                .ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        return SendResponseAsync(TelegramBotClient.WithCurrentUser);
                    }

                    if (task.IsFaulted || task.IsCanceled)
                    {
                        return SendErrorAsync(TelegramBotClient.WithCurrentUser, task.Exception)
                            .ContinueWith(_ => throw task.Exception);
                    }

                    return task;
                });

        protected abstract Task ProcessAsync();
        
        protected abstract Task<Message> SendResponseAsync(ICurrentTelegramUserClient client);
    }
    
    public abstract class CommandWithResponse<TResponse> : Command
    {
        protected CommandWithResponse(IExtendedTelegramBotClient telegramBotClient) : base(telegramBotClient)
        {
        }

        public override Task ExecuteAsync()
            => ProcessAsync()
                .ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        return SendResponseAsync(TelegramBotClient.WithCurrentUser, task.Result);
                    }

                    if (task.IsFaulted || task.IsCanceled)
                    {
                        return SendErrorAsync(TelegramBotClient.WithCurrentUser, task.Exception)
                            .ContinueWith(_ => throw task.Exception);
                    }

                    return task;
                });
        
        protected abstract Task<TResponse> ProcessAsync();
        protected abstract Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, TResponse response);
    }
}