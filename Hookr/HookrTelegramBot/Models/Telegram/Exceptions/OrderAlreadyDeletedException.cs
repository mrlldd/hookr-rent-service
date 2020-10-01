using HookrTelegramBot.Models.Telegram.Exceptions.Base;

namespace HookrTelegramBot.Models.Telegram.Exceptions
{
    public class OrderAlreadyDeletedException : TelegramException
    {
        public OrderAlreadyDeletedException(string message) : base(message)
        {
        }
    }
}