using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.User;
using HookrTelegramBot.Utilities.Telegram.Bot.Provider;
using HookrTelegramBot.Utilities.Telegram.Selectors;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Client
{
    public class ExtendedTelegramBotClient : DecoratedTelegramBotClientBase, IExtendedTelegramBotClient
    {
        public ExtendedTelegramBotClient(IChatSelector chatSelector,
            ICurrentUpdateProvider currentUpdateProvider,
            ITelegramBotProvider provider,
            bool omitEventProxies = true)
            : base(provider, omitEventProxies)
        {
            WithCurrentUser = new CurrentTelegramUserClient(Bot, chatSelector.Select(currentUpdateProvider.Instance));
        }

        public ICurrentTelegramUserClient WithCurrentUser { get; }
    }
}