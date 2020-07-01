using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser
{
    public class CurrentTelegramUserClient : ICurrentTelegramUserClient
    {
        private readonly ITelegramBotClient botClient;
        private readonly Chat chat;

        public CurrentTelegramUserClient(ITelegramBotClient botClient, Chat chat)
        {
            this.botClient = botClient;
            this.chat = chat;
        }

        public Task<Message> SendTextMessageAsync(string text,
            ParseMode parseMode = default,
            bool disableWebPagePreview = false,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup replyMarkup = null,
            CancellationToken cancellationToken = default)
            => botClient
                .SendTextMessageAsync(chat,
                    text,
                    parseMode,
                    disableWebPagePreview,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken);
    }
}