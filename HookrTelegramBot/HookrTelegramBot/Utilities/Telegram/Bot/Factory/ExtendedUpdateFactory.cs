using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Utilities.Telegram.Selectors.UpdateMessage;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Factory
{
    public class ExtendedUpdateFactory : IExtendedUpdateFactory
    {
        private readonly IUpdateMessageSelectorsContainer updateMessageSelectorsContainer;

        public ExtendedUpdateFactory(IUpdateMessageSelectorsContainer updateMessageSelectorsContainer)
        {
            this.updateMessageSelectorsContainer = updateMessageSelectorsContainer;
        }

        public ExtendedUpdate Create(Update update) 
            => new ExtendedUpdate(update, updateMessageSelectorsContainer.GetSelector(update));
    }
}