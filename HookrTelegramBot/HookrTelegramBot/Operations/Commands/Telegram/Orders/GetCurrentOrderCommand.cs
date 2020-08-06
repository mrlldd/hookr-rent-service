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
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders
{
    public class GetCurrentOrderCommand : CommandWithResponse<(OrderedTobacco[] Tobaccos, OrderedHookah[] Hookahs)>,
        IGetCurrentOrderCommand
    {
        private readonly ICurrentOrderCache currentOrderCache;
        private readonly IUserContextProvider userContextProvider;
        private readonly IHookrRepository hookrRepository;

        public GetCurrentOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            ICurrentOrderCache currentOrderCache,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository)
            : base(telegramBotClient)
        {
            this.currentOrderCache = currentOrderCache;
            this.userContextProvider = userContextProvider;
            this.hookrRepository = hookrRepository;
        }

        protected override Task<(OrderedTobacco[] Tobaccos, OrderedHookah[] Hookahs)> ProcessAsync()
        {
            var orderId = currentOrderCache.Get(userContextProvider.DatabaseUser.Id);
            return orderId.HasValue
                ? (
                    hookrRepository.ReadAsync((context, token)
                        => context.OrderedTobaccos
                            .Include(x => x.Product)
                            .Where(x => x.OrderId == orderId)
                            .ToArrayAsync(token)),
                    hookrRepository.ReadAsync((context, token)
                        => context.OrderedHookahs
                            .Include(x => x.Product)
                            .Where(x => x.OrderId == orderId)
                            .ToArrayAsync(token))
                )
                .CombineAsync()
                : Task.FromResult((Array.Empty<OrderedTobacco>(), Array.Empty<OrderedHookah>()));
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client,
            (OrderedTobacco[] Tobaccos, OrderedHookah[] Hookahs) response)
            => client
                .SendTextMessageAsync(!response.Hookahs.Any() || !response.Tobaccos.Any()
                    ? StringifyResponse(response)
                    : "seems like there is no order");

        private static string StringifyResponse((OrderedTobacco[] Tobaccos, OrderedHookah[] Hookahs) aggregatedOrder) =>
            (aggregatedOrder.Tobaccos.Any()
                ? "Tobaccos:" +
                  AggregateProducts(aggregatedOrder.Tobaccos)
                : string.Empty) +
            (aggregatedOrder.Hookahs.Any()
                ? "\n\nHookahs:" +
                  AggregateProducts(aggregatedOrder.Hookahs)
                : string.Empty);

        private static string AggregateProducts<TProduct>(IEnumerable<Ordered<TProduct>> products)
            where TProduct : Product
            => products
                .Aggregate(string.Empty,
                    (prev, next) => prev + $"\n{next.Product.Name} - {next.Count}");
    }
}