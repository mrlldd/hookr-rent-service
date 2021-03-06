﻿using System;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Models.Telegram.Exceptions;
using Hookr.Telegram.Models.Telegram.Exceptions.Base;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Serilog;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Operations.Base
{
    public abstract class Command : ICommand
    {
        protected readonly IExtendedTelegramBotClient TelegramBotClient;
        protected readonly ITranslationsResolver TranslationsResolver;

        protected Command(IExtendedTelegramBotClient telegramBotClient,
            ITranslationsResolver translationsResolver)
        {
            TelegramBotClient = telegramBotClient;
            TranslationsResolver = translationsResolver;
        }

        public abstract Task ExecuteAsync();

        private Exception? GetNetherException(Exception? exception)
            => exception is AggregateException aggregateException
                ? GetNetherException(aggregateException.InnerException)
                : exception;
        
        protected virtual async Task<Message> SendErrorAsync(ICurrentTelegramUserClient client, Exception exception)
        {
            //todo change to logger method call
            Log.Information(exception.ToString());
            string message;
            var netherException = GetNetherException(exception);
            if (netherException is TelegramException)
            {
                switch (netherException)
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
                        var (success, anotherMessage) = await ReadCustomExceptionAsync(netherException);
                        if (success)
                        {
                            message = anotherMessage;
                        }
                        else
                        {
                            message = await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.NotAvailable);
                        }

                        break;
                    }
                }
            }
            else
            {
                message = await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.NotAvailable);
            }

            return await client.SendTextMessageAsync(message);
        }

        protected virtual Task<(bool, string)> ReadCustomExceptionAsync(Exception exception) => Task.FromResult((false, string.Empty));
    }

    public abstract class CommandWithResponse : Command
    {
        protected CommandWithResponse(IExtendedTelegramBotClient telegramBotClient,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                translationsResolver)
        {
        }

        public sealed override async Task ExecuteAsync()
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
        protected CommandWithResponse(IExtendedTelegramBotClient telegramBotClient,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                translationsResolver)
        {
        }

        public sealed override async Task ExecuteAsync()
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