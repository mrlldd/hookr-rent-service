using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders
{
    public class AddToOrderCommand : CommandWithResponse, IAddToOrderCommand
    {
        private const string Space = " ";
        private readonly IHookrRepository hookrRepository;
        private readonly IUserContextProvider userContextProvider;

        public AddToOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserContextProvider userContextProvider) : base(telegramBotClient)
        {
            this.hookrRepository = hookrRepository;
            this.userContextProvider = userContextProvider;
        }

        protected override async Task ProcessAsync()
        {
            var (orderId, productName, productIndex) = ExtractArguments(userContextProvider.Update.Content);
            var order = await hookrRepository.ReadAsync((context, token) =>
                context.Orders
                    .Include(x => x.OrderedHookahs)
                    .Include(x => x.OrderedTobaccos)
                    .FirstOrDefaultAsync(x => x.Id == orderId, token));
            if (order == null)
            {
                throw new InvalidOperationException($"Missing order with id {orderId}");
            }

            switch (productName)
            {
                case nameof(Hookah):
                {
                    await AddProductToOrderAsync(order,
                        productIndex,
                        context => context.Hookahs,
                        x => x.OrderedHookahs);
                    break;
                }
                case nameof(Tobacco):
                {
                    await AddProductToOrderAsync(order,
                        productIndex,
                        context => context.Tobaccos,
                        x => x.OrderedTobaccos);
                    break;
                }
                default:
                {
                    throw new InvalidOperationException("Wrong product type " + productName);
                }
            }
            await hookrRepository.Context.SaveChangesAsync();
        }

        private async Task AddProductToOrderAsync<TProduct, TOrderedProduct>(
            Order order,
            int productIndex,
            Func<HookrContext, DbSet<TProduct>> productTableSelector,
            Func<Order, ICollection<TOrderedProduct>> orderedCollectionSelector)
            where TProduct : Product
            where TOrderedProduct : Ordered<TProduct>, new()
        {
            var products = await hookrRepository.ReadAsync((context, token) => productTableSelector(context)
                .ToArrayAsync(token));
            var product = products.ElementAt(productIndex);
            if (product == null)
            {
                throw new InvalidOperationException($"Missing {typeof(TProduct)} with index {productIndex}");
            }

            orderedCollectionSelector(order).Add(new TOrderedProduct
            {
                Product = product
            });
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => client.SendTextMessageAsync("Successfully added product to order.");


        private (int OrderId, string ProductName, int ProductIndex) ExtractArguments(string command)
        {
            var subs = command
                .Split(Space)
                .Skip(1)
                .ToArray();
            if (subs.Length != 3)
            {
                throw new InvalidOperationException("Wrong arguments have been passed in.");
            }

            var productName = subs[1];
            if (!(int.TryParse(subs[0], out var orderId)
                  && int.TryParse(subs[2], out var tobaccoIndex)
                  && (
                      productName.Equals(nameof(Tobacco))
                      || productName.Equals(nameof(Hookah)))
                ))
            {
                throw new InvalidOperationException("Wrong arguments have been passed in.");
            }

            return (orderId, productName, tobaccoIndex - 1);
        }
    }
}