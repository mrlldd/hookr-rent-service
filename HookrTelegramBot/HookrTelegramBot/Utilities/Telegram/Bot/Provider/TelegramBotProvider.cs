using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Provider
{
    public class TelegramBotProvider : ITelegramBotProvider
    {
        public TelegramBotProvider()
        {
            
        }
        public ITelegramBotClient Instance { get; private set; }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }
    }
}