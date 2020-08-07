using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
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
            => client
                .SendTextMessageAsync(response.OrderedHookahs.Any() || response.OrderedTobaccos.Any()
                        ? StringifyResponse(response)
                        : "seems like there is no any data in order",
                    replyMarkup: new InlineKeyboardMarkup(new[]
                    {
                        new InlineKeyboardButton
                        {
                            Text = "Delete",
                            CallbackData = $"/deleteorder {response.Id}"
                        },
                    }));

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
                    (prev, next) => prev + $"\n{next.Product.Name} - {next.Count}");

    }
}