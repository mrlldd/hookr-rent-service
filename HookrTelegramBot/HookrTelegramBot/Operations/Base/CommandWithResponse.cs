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

        protected virtual async Task<Message> SendErrorAsync(ICurrentTelegramUserClient client, Exception exception)
        {
            Log.Information(exception.ToString());
            var message = "Not available at the moment, sorry :(";
            var determinedException =
                exception is AggregateException aggregated
                    ? aggregated.InnerException
                    : exception;
            if (determinedException is TelegramException)
            {
                switch (determinedException)
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
                        var (success, anotherMessage) = await ReadCustomExceptionAsync(determinedException);
                        if (success)
                        {
                            message = anotherMessage;
                        }

                        break;
                    }
                }
            }

            return await client.SendTextMessageAsync(message);
        }

        protected virtual Task<(bool, string)> ReadCustomExceptionAsync(Exception exception) => Task.FromResult((false, string.Empty));
    }

    public abstract class CommandWithResponse : Command
    {
        protected CommandWithResponse(IExtendedTelegramBotClient telegramBotClient) : base(telegramBotClient)
        {
        }

        public override async Task ExecuteAsync()
        {
            try
            {
                await ProcessAsync();
                await SendResponseAsync(TelegramBotClient.WithCurrentUser);
            }
            catch (Exception e)
            {
                await SendErrorAsync(TelegramBotClient.WithCurrentUser, e);
                throw;
            }
        } 

        protected abstract Task ProcessAsync();

        protected abstract Task<Message> SendResponseAsync(ICurrentTelegramUserClient client);
    }

    public abstract class CommandWithResponse<TResponse> : Command
    {
        protected CommandWithResponse(IExtendedTelegramBotClient telegramBotClient) : base(telegramBotClient)
        {
        }

        public override async Task ExecuteAsync()
        {
            try
            {
                await SendResponseAsync(TelegramBotClient.WithCurrentUser, await ProcessAsync());
            }
            catch (Exception e)
            {
                await SendErrorAsync(TelegramBotClient.WithCurrentUser, e);
                throw;
            }
        }

        protected abstract Task<TResponse> ProcessAsync();
        protected abstract Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, TResponse response);
    }
}