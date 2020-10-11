using System;
using System.Linq;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Operations.Commands.Orders.Control.Service.Review.Confirmed.Approve;
using Hookr.Telegram.Operations.Commands.Orders.Control.Service.Review.Confirmed.Reject;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Extensions;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Notifiers;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Hookr.Telegram.Operations.Commands.Orders.Control.Confirm
{
    public class ConfirmOrderCommand : OrderCommandBase, IConfirmOrderCommand
    {
        private readonly ITelegramUsersNotifier telegramUsersNotifier;

        public ConfirmOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            ITelegramHookrRepository hookrRepository,
            ITelegramUsersNotifier telegramUsersNotifier,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository,
                translationsResolver)
        {
            this.telegramUsersNotifier = telegramUsersNotifier;
        }

        protected override async Task<Order> ProcessAsync(Order order)
        {
            order.State = OrderStates.Confirmed;
            var (
                notificationFormat,
                fromFormat,
                hookahRowFormat,
                tobaccoRowFormat,
                unknownTranslated,
                approveTranslated,
                rejectTranslated
                ) = await
                TranslationsResolver.ResolveAsync(
                    (TelegramTranslationKeys.NewOrderNotification, Array.Empty<object>()),
                    (TelegramTranslationKeys.From, Array.Empty<object>()),
                    (TelegramTranslationKeys.OrderNotificationHookahRow, Array.Empty<object>()),
                    (TelegramTranslationKeys.OrderNotificationTobaccoRow, Array.Empty<object>()),
                    (TelegramTranslationKeys.Unknown, Array.Empty<object>()),
                    (TelegramTranslationKeys.Approve, Array.Empty<object>()),
                    (TelegramTranslationKeys.Reject, Array.Empty<object>())
                );
            var from = UserContextProvider.Update.From;
            var notification = string.Format(notificationFormat,
                order.Id,
                //todo rework as there is a lot of warning about possible empty pointer dereference. 
                order.OrderedHookahs
                    .AggregateListString(hookahRowFormat,
                        x => x.Product.Name,
                        x => x.Product.Price,
                        x => x.Count,
                        x => x.Count * x.Product.Price),
                order.OrderedTobaccos
                    .AggregateListString(tobaccoRowFormat,
                        x => x.Product.Name,
                        x => x.Product.Price,
                        x => x.Count,
                        x => x.Count * x.Product.Price),
                string.Format(fromFormat,
                    string.IsNullOrEmpty(from.Username)
                        ? $"[{unknownTranslated}]"
                        : $"@{from.Username}",
                    string.IsNullOrEmpty(from.FirstName)
                        ? $"[{unknownTranslated}]"
                        : from.FirstName,
                    string.IsNullOrEmpty(from.LastName)
                        ? $"[{unknownTranslated}]"
                        : from.LastName)
            );
            await HookrRepository.Context.SaveChangesAsync();
            await telegramUsersNotifier.SendAsync((client, user) =>
                    client.SendTextMessageAsync(user.Id,
                        notification,
                        ParseMode.MarkdownV2,
                        replyMarkup: new InlineKeyboardMarkup(new[]
                        {
                            new InlineKeyboardButton
                            {
                                Text = approveTranslated,
                                CallbackData = $"/{nameof(ApproveOrderCommand).ExtractCommandName()} {order.Id}"
                            },
                            new InlineKeyboardButton
                            {
                                Text = rejectTranslated,
                                CallbackData = $"/{nameof(RejectOrderCommand).ExtractCommandName()} {order.Id}"
                            }
                        })),
                TelegramUserStates.Service,
                TelegramUserStates.Dev);
            return order;
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Order response)
            => await client.SendTextMessageAsync(
                await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.ConfirmOrderSuccess, response.Id));

        protected override Task CustomOrderAsyncValidator(Order order, TelegramUser user)
        {
            if (order.State != OrderStates.Constructing)
            {
                throw new InvalidOperationException("Order is already confirmed.");
            }

            return Task.CompletedTask;
        }

        protected override IQueryable<Order> SideQuery(IQueryable<Order> query)
            => query
                .Include(x => x.OrderedHookahs)
                .ThenInclude(x => x.Product)
                .Include(x => x.OrderedTobaccos)
                .ThenInclude(x => x.Product);
    }
}