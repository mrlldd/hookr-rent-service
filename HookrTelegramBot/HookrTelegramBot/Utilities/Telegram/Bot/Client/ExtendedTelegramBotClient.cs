using HookrTelegramBot.Utilities.Telegram.Bot.Provider;
using HookrTelegramBot.Utilities.Telegram.Selectors;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Client
{
    public class ExtendedTelegramBotClient : DecoratedTelegramBotClientBase, IExtendedTelegramBotClient
    {
        private readonly IChatSelector chatSelector;

        public ExtendedTelegramBotClient(IChatSelector chatSelector, ITelegramBotProvider provider, bool omitEventProxies = true) : base(provider, omitEventProxies)
        {
            this.chatSelector = chatSelector;
        }
    }
}