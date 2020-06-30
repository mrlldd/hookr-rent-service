using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Selectors;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Base
{
    public abstract class CommandWithResponse : ICommand
    {
        private readonly IExtendedTelegramBotClient telegramBotClient;
        protected readonly ICurrentUpdateProvider currentUpdateProvider;

        protected CommandWithResponse(IExtendedTelegramBotClient telegramBotClient, ICurrentUpdateProvider currentUpdateProvider)
        {
            this.telegramBotClient = telegramBotClient;
            this.currentUpdateProvider = currentUpdateProvider;
        }

        public Task ExecuteAsync()
            => ProcessAsync()
                .ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        return SendResponseAsync(telegramBotClient);
                    }

                    if (task.IsFaulted || task.IsCanceled)
                    {
                        throw task.Exception;
                    }

                    return task;
                });

        protected abstract Task ProcessAsync();
        protected abstract Task<Message> SendResponseAsync(IExtendedTelegramBotClient bot);
    }
}