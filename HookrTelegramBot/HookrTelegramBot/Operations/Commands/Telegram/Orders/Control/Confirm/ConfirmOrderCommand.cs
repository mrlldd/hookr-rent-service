﻿using System;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Service.Review.Confirmed.Approve;
using HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Service.Review.Confirmed.Reject;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Notifiers;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Confirm
{
    public class ConfirmOrderCommand : OrderCommandBase, IConfirmOrderCommand
    {
        private readonly ITelegramUsersNotifier telegramUsersNotifier;

        public ConfirmOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
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
            //todo notify service users about new order;
            var (notificationFormat, fromFormat, hookahRowFormat, tobaccoRowFormat, unknownTranslated) = await
                TranslationsResolver.ResolveAsync(
                    (TranslationKeys.NewOrderNotification, Array.Empty<object>()),
                    (TranslationKeys.From, Array.Empty<object>()),
                    (TranslationKeys.OrderNotificationHookahRow, Array.Empty<object>()),
                    (TranslationKeys.OrderNotificationTobaccoRow, Array.Empty<object>()),
                    (TranslationKeys.Unknown, Array.Empty<object>())
                );
            var from = UserContextProvider.Update.From;
            var notification = string.Format(notificationFormat,
                order.Id,
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
                                Text = "Approve",
                                CallbackData = $"/{nameof(ApproveOrderCommand).ExtractCommandName()} {order.Id}"
                            }, 
                            new InlineKeyboardButton
                            {
                                Text = "Reject",
                                CallbackData = $"/{nameof(RejectOrderCommand).ExtractCommandName()} {order.Id}"
                            }, 
                        })),
                TelegramUserStates.Service,
                TelegramUserStates.Dev);
            return order;
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Order response)
            => await client.SendTextMessageAsync(
                await TranslationsResolver.ResolveAsync(TranslationKeys.ConfirmOrderSuccess, response.Id));

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