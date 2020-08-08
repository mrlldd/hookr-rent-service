﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Confirm;
using HookrTelegramBot.Operations.Commands.Telegram.Orders.Delete;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Caches.CurrentOrder;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders.Get
{
    public class GetOrderCommand : OrderCommandBase,
        IGetOrderCommand
    {
        public GetOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository)
        {
        }

        protected override IQueryable<Order> SideQuery(IQueryable<Order> query)
            => query
                .Include(x => x.OrderedHookahs)
                .ThenInclude(x => x.Product)
                .Include(x => x.OrderedTobaccos)
                .ThenInclude(x => x.Product);

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client,
            Order response)
        {
            var buttons = new List<InlineKeyboardButton>();
            string text;
            if (response.OrderedHookahs.Any() || response.OrderedTobaccos.Any())
            {
                text = StringifyResponse(response);
                if (response.State == OrderStates.Constructing)
                {
                    buttons.Add(new InlineKeyboardButton
                    {
                        Text = "Confirm",
                        CallbackData = $"/{nameof(ConfirmOrderCommand).ExtractCommandName()} {response.Id}"
                    });
                }
            }
            else
            {
                text = "seems like there is no any data in order";
            }

            text += $"\n\nStatus: *{response.State}*";
            if (response.State == OrderStates.Constructing)
            {
                buttons.Add(new InlineKeyboardButton
                {
                    Text = "Delete",
                    CallbackData = $"/{nameof(DeleteOrderCommand).ExtractCommandName()} {response.Id}"
                });
            }

            return client
                .SendTextMessageAsync(text,
                    ParseMode.MarkdownV2,
                    replyMarkup: new InlineKeyboardMarkup(buttons));
        }

        private static string StringifyResponse(Order order) =>
            (order.OrderedTobaccos.Any()
                ? "Tobaccos:" +
                  AggregateProducts(order.OrderedTobaccos)
                : string.Empty) +
            (order.OrderedHookahs.Any()
                ? "\n\nHookahs:" +
                  AggregateProducts(order.OrderedHookahs)
                : string.Empty);

        private static string AggregateProducts<TProduct>(IEnumerable<Ordered<TProduct>> products)
            where TProduct : Product
            => products
                .Aggregate(string.Empty,
                    (prev, next) => prev + $"\n{next.Product.Name} \\- {next.Count}");

    }
}