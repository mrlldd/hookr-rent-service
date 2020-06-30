using System.Threading.Tasks;
using HookrTelegramBot.Utilities.Telegram.Bot.Provider;
using HookrTelegramBot.Utilities.Telegram.Selectors;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Client
{
    public class ExtendedTelegramBotClient : DecoratedTelegramBotClientBase, IExtendedTelegramBotClient
    {
        private readonly IChatSelector chatSelector;
        private readonly ICurrentUpdateProvider currentUpdateProvider;

        public ExtendedTelegramBotClient(IChatSelector chatSelector,
            ICurrentUpdateProvider currentUpdateProvider,
            ITelegramBotProvider provider,
            bool omitEventProxies = true) : base(provider, omitEventProxies)
        {
            this.chatSelector = chatSelector;
            this.currentUpdateProvider = currentUpdateProvider;
        }

        public Task<Message> SendTextMessageToCurrentUserAsync(string text)
            => Bot.SendTextMessageAsync(chatSelector.Select(currentUpdateProvider.Instance), text);
    }
}