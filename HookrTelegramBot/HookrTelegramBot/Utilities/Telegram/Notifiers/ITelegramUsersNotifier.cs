using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Notifiers
{
    public interface ITelegramUsersNotifier
    {
        Task<IEnumerable<Message>> SendAsync(Func<IExtendedTelegramBotClient, TelegramUser, Task<Message>> functor,
            params TelegramUserStates[] userTypes);
    }
}