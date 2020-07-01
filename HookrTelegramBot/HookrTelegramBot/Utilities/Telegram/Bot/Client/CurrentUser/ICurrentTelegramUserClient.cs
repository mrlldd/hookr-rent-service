﻿using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser
{
    public interface ICurrentTelegramUserClient
    {
        Task<Message> SendTextMessageAsync(string text,
            ParseMode parseMode = default,
            bool disableWebPagePreview = false,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup replyMarkup = null,
            CancellationToken cancellationToken = default);
    }
}