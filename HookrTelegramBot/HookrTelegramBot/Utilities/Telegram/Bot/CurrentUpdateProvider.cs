using System;
using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Bot
{
    public class CurrentUpdateProvider : ICurrentUpdateProvider
    {
        public ExtendedUpdate Instance { get; private set; }
        public void Set(Update update)
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("Update is already set.");
            }

            Instance = new ExtendedUpdate(update);
        }
    }
}