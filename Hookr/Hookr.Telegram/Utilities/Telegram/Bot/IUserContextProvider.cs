using System.Diagnostics.CodeAnalysis;
using Hookr.Telegram.Models.Telegram;
using Hookr.Telegram.Repository.Context.Entities.Base;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Utilities.Telegram.Bot
{
    public interface IUserContextProvider
    {
        [NotNull]
        ExtendedUpdate? Update { get; }
        [NotNull]
        TelegramUser? DatabaseUser { get; }
        ExtendedUpdate Set(Update update);
        void SetDatabaseUser(TelegramUser telegramUser);
    }
}