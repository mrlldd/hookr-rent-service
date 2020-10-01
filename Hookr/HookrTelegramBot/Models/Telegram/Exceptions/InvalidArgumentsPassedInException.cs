using HookrTelegramBot.Models.Telegram.Exceptions.Base;

namespace HookrTelegramBot.Models.Telegram.Exceptions
{
    public class InvalidArgumentsPassedInException : TelegramException
    {
        public InvalidArgumentsPassedInException(string message) : base(message)
        {
        }
    }
}