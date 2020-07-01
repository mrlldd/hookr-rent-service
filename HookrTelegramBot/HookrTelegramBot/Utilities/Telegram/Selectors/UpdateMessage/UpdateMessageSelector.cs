using System;
using System.Collections.Generic;
using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HookrTelegramBot.Utilities.Telegram.Selectors.UpdateMessage
{
    public class UpdateMessageSelector : IUpdateMessageSelector
    {
        private readonly IDictionary<UpdateType, Func<ExtendedUpdate, Message>> selectors =
            new Dictionary<UpdateType, Func<ExtendedUpdate, Message>>
            {
                {UpdateType.Message, update => update.Message},
                {UpdateType.CallbackQuery, update => update.CallbackQuery.Message},
                {UpdateType.ChannelPost, update => update.ChannelPost},
                {UpdateType.EditedMessage, update => update.EditedMessage},
                {UpdateType.EditedChannelPost, update => update.EditedChannelPost}
            };

        public Message Select(ExtendedUpdate update)
            => selectors.TryGetValue(update.Type, out var selector)
                ? selector(update)
                : throw new ArgumentOutOfRangeException(update.Type.ToString());
    }
}