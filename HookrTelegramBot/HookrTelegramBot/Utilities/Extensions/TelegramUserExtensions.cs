using HookrTelegramBot.Repository.Context.Entities.Base;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Extensions
{
    public static class TelegramUserExtensions
    {
        public static TelegramUser ToDatabaseUser(this User user)
            => new TelegramUser
            {
                Id = user.Id,
                Username = user.Username
            }; 
    }
}