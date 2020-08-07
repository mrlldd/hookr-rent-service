using HookrTelegramBot.Models.Telegram.Exceptions.Base;

namespace HookrTelegramBot.Models.Telegram.Exceptions
{
    public class InsufficientAccessRightsException : TelegramException
    {
        public InsufficientAccessRightsException(string message) : base(message)
        {
        }
    }
}