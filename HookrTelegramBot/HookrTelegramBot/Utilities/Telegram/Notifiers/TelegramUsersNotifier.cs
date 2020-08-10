using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Notifiers
{
    public class TelegramUsersNotifier : ITelegramUsersNotifier
    {
        private readonly IHookrRepository hookrRepository;
        private readonly IExtendedTelegramBotClient telegramBotClient;

        public TelegramUsersNotifier(IHookrRepository hookrRepository,
            IExtendedTelegramBotClient telegramBotClient)
        {
            this.hookrRepository = hookrRepository;
            this.telegramBotClient = telegramBotClient;
        }

        public async Task<IEnumerable<Message>> SendAsync(Func<IExtendedTelegramBotClient, TelegramUser, Task<Message>> functor,
            params TelegramUserStates[] usersTypes)
        {
            if (usersTypes.Length == 0)
            {
                throw new ArgumentException("Unknown users type to notify.", nameof(usersTypes));
            }
            //todo caching for nondefault users.
            var users = await hookrRepository
                .ReadAsync((context, token) => context.TelegramUsers
                    .Where(x => usersTypes.Contains(x.State))
                    .ToArrayAsync(token));
            return await users
                .Select(x => new Func<Task<Message>>(() => functor(telegramBotClient, x)))
                .ExecuteMultipleAsync();
        }
    }
}