using System;
using System.Diagnostics.CodeAnalysis;
using Hookr.Telegram.Models.Telegram;
using Hookr.Telegram.Repository.Context.Entities.Base;
using Hookr.Telegram.Utilities.Telegram.Bot.Factory;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Utilities.Telegram.Bot
{
    public class UserContextProvider : IUserContextProvider
    {
        private readonly IExtendedUpdateFactory extendedUpdateFactory;

        public UserContextProvider(IExtendedUpdateFactory extendedUpdateFactory)
        {
            this.extendedUpdateFactory = extendedUpdateFactory;
        }
        [NotNull]
        public ExtendedUpdate? Update { get; private set; }
        [NotNull]
        public TelegramUser? DatabaseUser { get; private set; }
        [return: NotNull]
        public ExtendedUpdate Set(Update update)
        {
            if (Update != null)
            {
                throw new InvalidOperationException("Update is already set.");
            }

            return Update = extendedUpdateFactory.Create(update);
        }

        public void SetDatabaseUser(TelegramUser telegramUser)
            => DatabaseUser = telegramUser;
    }
}