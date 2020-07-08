using System;
using System.Collections.Generic;
using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HookrTelegramBot.Utilities.Telegram.Selectors.UpdateMessage
{
    public class UpdateMessageSelectorsContainer : IUpdateMessageSelectorsContainer
    {
        private readonly IDictionary<UpdateType, Func<Update, Message>> selectors =
            new Dictionary<UpdateType, Func<Update, Message>>
            {
                {UpdateType.Message, update => update.Message},
                {UpdateType.CallbackQuery, update => update.CallbackQuery.Message},
                {UpdateType.ChannelPost, update => update.ChannelPost},
                {UpdateType.EditedMessage, update => update.EditedMessage},
                {UpdateType.EditedChannelPost, update => update.EditedChannelPost}
            };

        public Func<Update, Message> GetSelector(Update update)
            => selectors.TryGetValue(update.Type, out var selector)
                ? selector
                : throw new ArgumentOutOfRangeException(update.Type.ToString());
        
    }
}