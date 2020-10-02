using Hookr.Telegram.Models.Telegram.Exceptions.Base;

namespace Hookr.Telegram.Models.Telegram.Exceptions
{
    public class InsufficientAccessRightsException : TelegramException
    {
        public InsufficientAccessRightsException(string message) : base(message)
        {
        }
    }
}