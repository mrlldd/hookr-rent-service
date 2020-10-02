using System;

namespace Hookr.Telegram.Models.Telegram.Exceptions.Base
{
    public abstract class TelegramException : Exception
    {
        protected TelegramException(string message) : base(message)
        {
            
        }
    }
}