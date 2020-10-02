using System.Threading;
using System.Threading.Tasks;
using Hookr.Telegram.Utilities.Extensions;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Bot.Provider;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Hookr.Telegram.Utilities.Telegram.Bot.Client
{
    public class ExtendedTelegramBotClient : DecoratedTelegramBotClientBase, IExtendedTelegramBotClient
    {
        public ExtendedTelegramBotClient(
            IUserContextProvider userContextProvider,
            ITelegramBotProvider provider,
            
            bool omitEventProxies = true)
            : base(provider, omitEventProxies)
        {
            WithCurrentUser = new CurrentTelegramUserClient(Bot, userContextProvider);
        }

        public ICurrentTelegramUserClient WithCurrentUser { get; }

        public override Task<Message> SendTextMessageAsync(ChatId chatId,
            string text,
            ParseMode parseMode = ParseMode.Default,
            bool disableWebPagePreview = false,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => base
                .SendTextMessageAsync(chatId,
                    parseMode == ParseMode.MarkdownV2
                        ? text.FilterPreservedCharacters()
                        : text,
                    parseMode,
                    disableWebPagePreview,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken);
    }
}