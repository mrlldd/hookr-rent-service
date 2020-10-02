using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser
{
    public interface ICurrentTelegramUserClient
    {
        User User { get; }
        Task<Message> SendTextMessageAsync(string text,
            ParseMode parseMode = default,
            bool disableWebPagePreview = false,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default);

        Task<Message> SendTextMessageAsync(Func<Task<string>> contentProducer,
            ParseMode parseMode = default,
            bool disableWebPagePreview = false,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default);

        Task<Message[]> SendMediaGroupAsync(
            IEnumerable<IAlbumInputMedia> inputMedia,
            bool disableNotification = default,
            int replyToMessageId = default,
            CancellationToken cancellationToken = default);
    }
}