using System;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Utilities.Telegram.Bot.Factory;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Bot
{
    public class UserContextProvider : IUserContextProvider
    {
        private readonly IExtendedUpdateFactory extendedUpdateFactory;

        public UserContextProvider(IExtendedUpdateFactory extendedUpdateFactory)
        {
            this.extendedUpdateFactory = extendedUpdateFactory;
        }

        public ExtendedUpdate Update { get; private set; }
        public ExtendedUpdate Set(Update update)
        {
            if (Update != null)
            {
                throw new InvalidOperationException("Update is already set.");
            }

            return Update = extendedUpdateFactory.Create(update);
        }
    }
}