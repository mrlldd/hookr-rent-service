using System.Threading;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser
{
    public class CurrentTelegramUserClient : ICurrentTelegramUserClient
    {
        private readonly ITelegramBotClient botClient;
        private readonly ExtendedUpdate update;

        public CurrentTelegramUserClient(ITelegramBotClient botClient, ExtendedUpdate update)
        {
            this.botClient = botClient;
            this.update = update;
        }

        public Task<Message> SendTextMessageAsync(string text,
            ParseMode parseMode = default,
            bool disableWebPagePreview = false,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup replyMarkup = null,
            CancellationToken cancellationToken = default)
            => botClient
                .SendTextMessageAsync(update.Chat,
                    text,
                    parseMode,
                    disableWebPagePreview,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken);
    }
}