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

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders.Get
{
    public class GetOrderCommand : CommandWithResponse<(OrderedTobacco[] Tobaccos, OrderedHookah[] Hookahs)>,
        IGetOrderCommand
    {
        private readonly ICurrentOrderCache currentOrderCache;
        private readonly IUserContextProvider userContextProvider;
        private readonly IHookrRepository hookrRepository;

        private const string Space = " ";

        public GetOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            ICurrentOrderCache currentOrderCache,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository)
            : base(telegramBotClient)
        {
            this.currentOrderCache = currentOrderCache;
            this.userContextProvider = userContextProvider;
            this.hookrRepository = hookrRepository;
        }

        protected override async Task<(OrderedTobacco[] Tobaccos, OrderedHookah[] Hookahs)> ProcessAsync()
        {
            var orderId = ExtractArguments(userContextProvider.Update.Content);
            var order = await hookrRepository.ReadAsync((context, token) =>
                context.Orders.FirstOrDefaultAsync(x => x.Id == orderId, token));
            if (order == null || order.CreatedById != userContextProvider.DatabaseUser.Id)
            {
                throw new InvalidOperationException("Seems like you have no access :(");
            }

            return await (
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
                .CombineAsync();
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client,
            (OrderedTobacco[] Tobaccos, OrderedHookah[] Hookahs) response)
            => client
                .SendTextMessageAsync(response.Hookahs.Any() || response.Tobaccos.Any()
                    ? StringifyResponse(response)
                    : "seems like there is no any data in order");

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

        private static int ExtractArguments(string command)
        {
            var subs = command.Split(Space);
            if (subs.Length != 2)
            {
                throw new InvalidOperationException("Wrong arguments have been passed in.");
            }

            return int.TryParse(subs.Last(), out var result)
                ? result
                : throw new InvalidOperationException("Wrong argument have been passed in.");
        }
    }
}