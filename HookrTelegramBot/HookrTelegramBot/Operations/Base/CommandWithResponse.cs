using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Utilities.Telegram.Selectors;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Base
{
    public abstract class CommandWithResponse : ICommand
    {
        private readonly IChatSelector chatSelector;

        protected CommandWithResponse(IChatSelector chatSelector)
        {
            this.chatSelector = chatSelector;
        }

        public Task ExecuteAsync(ExtendedUpdate update)
            => ProcessAsync(update)
                .ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        return SendResponseAsync(chatSelector.Select(update));
                    }

                    if (task.IsFaulted || task.IsCanceled)
                    {
                        throw task.Exception;
                    }

                    return task;
                });

        protected abstract Task ProcessAsync(ExtendedUpdate update);
        protected abstract Task<Message> SendResponseAsync(Chat chat);
    }
}