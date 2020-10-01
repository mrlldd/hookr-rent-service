using System;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot.Factory;
using Microsoft.EntityFrameworkCore;
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
        public TelegramUser DatabaseUser { get; private set; }
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