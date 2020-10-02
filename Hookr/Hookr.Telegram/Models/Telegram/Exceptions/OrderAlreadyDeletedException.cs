using Hookr.Telegram.Models.Telegram.Exceptions.Base;

namespace Hookr.Telegram.Models.Telegram.Exceptions
{
    public class OrderAlreadyDeletedException : TelegramException
    {
        public OrderAlreadyDeletedException(string message) : base(message)
        {
        }
    }
}