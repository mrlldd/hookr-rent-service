using System.Diagnostics.CodeAnalysis;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Telegram.Models.Telegram;
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