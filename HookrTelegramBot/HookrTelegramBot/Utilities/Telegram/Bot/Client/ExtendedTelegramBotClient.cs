using System.Threading;
using System.Threading.Tasks;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Bot.Provider;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Client
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
            IReplyMarkup replyMarkup = null,
            CancellationToken cancellationToken = default)
        {
            var content = text;
            if (parseMode == ParseMode.MarkdownV2)
            {
                content = content.FilterPreservedCharacters();
            }
            return base
                .SendTextMessageAsync(chatId,
                    content,
                    parseMode,
                    disableWebPagePreview,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken);
        }
    }
}