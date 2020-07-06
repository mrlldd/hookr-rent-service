using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HookrTelegramBot.Models.Telegram
{
    public class ExtendedUpdate : Update
    {
        private readonly Func<Update, Message> realMessageSelector;
        /// <summary>
        /// Override as original UpdateType is not constant
        /// </summary>
        public new UpdateType Type { get; }

        public Message RealMessage => realMessageSelector(this);
        public Chat Chat => RealMessage.Chat;

        public ExtendedUpdate(Update update, Func<Update, Message> realMessageSelector)
        {
            Type = update.Type;
            Initialize(update);
            this.realMessageSelector = realMessageSelector;
        }

        private void Initialize(Update update)
        {
            switch (Type)
            {
                case UpdateType.Unknown:
                {
                    break;
                }
                case UpdateType.Message:
                {
                    Message = update.Message;
                    break;
                }
                case UpdateType.InlineQuery:
                {
                    InlineQuery = update.InlineQuery;
                    break;
                }
                case UpdateType.ChosenInlineResult:
                {
                    ChosenInlineResult = update.ChosenInlineResult;
                    break;
                }
                case UpdateType.CallbackQuery:
                {
                    CallbackQuery = update.CallbackQuery;
                    break;
                }
                case UpdateType.EditedMessage:
                {
                    EditedMessage = update.EditedMessage;
                    break;
                }
                case UpdateType.ChannelPost:
                {
                    ChannelPost = update.ChannelPost;
                    break;
                }
                case UpdateType.EditedChannelPost:
                {
                    EditedChannelPost = update.EditedChannelPost;
                    break;
                }
                case UpdateType.ShippingQuery:
                {
                    ShippingQuery = update.ShippingQuery;
                    break;
                }
                case UpdateType.PreCheckoutQuery:
                {
                    PreCheckoutQuery = update.PreCheckoutQuery;
                    break;
                }
                case UpdateType.Poll:
                {
                    Poll = update.Poll;
                    break;
                }
                case UpdateType.PollAnswer:
                {
                    PollAnswer = PollAnswer;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(update.Type));
            }
        }
    }
}