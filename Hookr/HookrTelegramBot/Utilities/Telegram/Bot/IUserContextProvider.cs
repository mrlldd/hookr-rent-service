using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Repository.Context.Entities.Base;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Bot
{
    public interface IUserContextProvider
    {
        ExtendedUpdate Update { get; }
        TelegramUser DatabaseUser { get; }
        ExtendedUpdate Set(Update update);
        void SetDatabaseUser(TelegramUser telegramUser);
    }
}