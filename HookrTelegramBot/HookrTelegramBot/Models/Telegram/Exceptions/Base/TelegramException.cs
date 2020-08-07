using System;

namespace HookrTelegramBot.Models.Telegram.Exceptions.Base
{
    public abstract class TelegramException : Exception
    {
        protected TelegramException(string message) : base(message)
        {
            
        }
    }
}