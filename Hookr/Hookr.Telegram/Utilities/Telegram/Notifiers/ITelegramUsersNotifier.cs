using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hookr.Telegram.Repository.Context.Entities.Base;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Utilities.Telegram.Notifiers
{
    public interface ITelegramUsersNotifier
    {
        Task<IEnumerable<Message>> SendAsync(Func<IExtendedTelegramBotClient, TelegramUser, Task<Message>> functor,
            params TelegramUserStates[] userTypes);

        Task<IEnumerable<Message>> SendAsync(
            Func<IExtendedTelegramBotClient, TelegramUser, Task<Message>> functor,
            params TelegramUser[] users);
    }
}