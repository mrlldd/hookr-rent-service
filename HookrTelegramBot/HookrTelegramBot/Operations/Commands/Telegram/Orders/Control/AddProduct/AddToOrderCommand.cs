using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram.Exceptions;
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

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.AddProduct
{
    public class AddToOrderCommand : OrderCommandBase, IAddToOrderCommand
    {
        public AddToOrderCommand(IExtendedTelegramBotClient telegramBotClient,
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
                .Include(x => x.OrderedTobaccos);

        protected override async Task<Order> ProcessAsync(Order order)
        {
            var (productName, productIndex) = ExtractProductProperties(ArgumentsLeft);
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

            await HookrRepository.Context.SaveChangesAsync();
            return order;
        }

        private static (string ProductName, int ProductIndex) ExtractProductProperties(string[] args)
        {
            if (args.Length != 2)
            {
                throw new InvalidArgumentsPassedInException("Wrong arguments have been passed in.");
            }
            var productName = args[0];
            if (!(int.TryParse(args[1], out var tobaccoIndex)
                                 && 
                                     (productName.Equals(nameof(Tobacco))
                                     || productName.Equals(nameof(Hookah)))
                                 ))
            {
                throw new InvalidArgumentsPassedInException("Wrong arguments have been passed in.");
            }

            return (
                productName,
                tobaccoIndex - 1
            );
        }

        private async Task AddProductToOrderAsync<TProduct, TOrderedProduct>(
            Order order,
            int productIndex,
            Func<HookrContext, DbSet<TProduct>> productTableSelector,
            Func<Order, ICollection<TOrderedProduct>> orderedCollectionSelector)
            where TProduct : Product
            where TOrderedProduct : Ordered<TProduct>, new()
        {
            var products = await HookrRepository.ReadAsync((context, token) => productTableSelector(context)
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

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Order response)
            => client.SendTextMessageAsync($"Successfully added product to order {response.Id}.");
    }
}