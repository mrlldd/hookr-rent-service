using System;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Selectors;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Base
{

    public class Command : ICommand
    {
        protected readonly IExtendedTelegramBotClient TelegramBotClient;

        protected Command(IExtendedTelegramBotClient telegramBotClient)
        {
            TelegramBotClient = telegramBotClient;
        }

        public virtual Task ExecuteAsync()
        {
            throw new NotImplementedException();
        }

        protected virtual Task<Message> SendErrorAsync(ICurrentTelegramUserClient client, Exception exception)
            => client.SendTextMessageAsync("Not available at the moment, sorry :(");

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
                .ContinueWith<Task>(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        return SendResponseAsync(TelegramBotClient.WithCurrentUser, task.Result);
                    }

                    if (task.IsFaulted || task.IsCanceled)
                    {
                        throw task.Exception;
                    }

                    return task;
                });
        
        protected abstract Task<TResponse> ProcessAsync();
        protected abstract Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, TResponse response);
    }
}