using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Utilities.Selectors;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Base
{
    public abstract class CommandWithResponse<TUpdate> : Command<TUpdate> where TUpdate : ExtendedUpdate
    {
        private readonly IChatSelector chatSelector;
        public abstract override string Name { get; }

        protected CommandWithResponse(IChatSelector chatSelector)
        {
            this.chatSelector = chatSelector;
        }

        public override async Task ExecuteAsync(TUpdate update)
        {
            await ProcessAsync(update);
            await SendResponseAsync(chatSelector.Select(update));
        }

        protected abstract Task ProcessAsync(TUpdate update);
        protected abstract Task<Message> SendResponseAsync(Chat chat);
    }
}