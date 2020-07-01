using System;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Utilities.Telegram.Selectors.UpdateMessage;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Bot
{
    public class UserContextProvider : IUserContextProvider
    {
        private readonly IUpdateMessageSelector updateMessageSelector;

        public UserContextProvider(IUpdateMessageSelector updateMessageSelector)
        {
            this.updateMessageSelector = updateMessageSelector;
        }

        public ExtendedUpdate Update { get; private set; }
        private Message RealMessage { get; set; }
        public Message Message => RealMessage ??= updateMessageSelector.Select(Update);
        public void Set(Update update)
        {
            if (Update != null)
            {
                throw new InvalidOperationException("Update is already set.");
            }

            Update = new ExtendedUpdate(update);
        }
    }
}