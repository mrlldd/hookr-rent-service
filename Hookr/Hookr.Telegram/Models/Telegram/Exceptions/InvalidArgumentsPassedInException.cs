using Hookr.Telegram.Models.Telegram.Exceptions.Base;

namespace Hookr.Telegram.Models.Telegram.Exceptions
{
    public class InvalidArgumentsPassedInException : TelegramException
    {
        public InvalidArgumentsPassedInException(string message) : base(message)
        {
        }
    }
}