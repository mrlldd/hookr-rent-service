using System;
using System.Collections;
using System.Collections.Generic;
using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HookrTelegramBot.Utilities.Selectors
{
    public class ChatSelector : IChatSelector
    {
        private readonly IDictionary<UpdateType, Func<ExtendedUpdate, Chat>> selectors =
            new Dictionary<UpdateType, Func<ExtendedUpdate, Chat>>
            {
                { UpdateType.Message, update => update.Message.Chat },
                { UpdateType.CallbackQuery, update => update.CallbackQuery.Message.Chat },
                { UpdateType.ChannelPost, update => update.ChannelPost.Chat },
                { UpdateType.EditedMessage, update => update.EditedMessage.Chat },
                { UpdateType.EditedChannelPost, update => update.EditedChannelPost.Chat }
            };

        public Chat Select<TUpdate>(TUpdate update) where TUpdate : ExtendedUpdate
            => selectors.TryGetValue(update.Type, out var selector)
                ? selector(update)
                : throw new ArgumentOutOfRangeException(update.Type.ToString());
    }
}