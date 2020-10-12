using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context;
using Hookr.Core.Repository.Context.Entities;
using Hookr.Core.Repository.Context.Entities.Products;
using Hookr.Core.Repository.Context.Entities.Products.Ordered;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Models.Telegram.Exceptions;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Operations.Commands.Orders.Control.AddProduct
{
    public class AddToOrderCommand : OrderCommandBase, IAddToOrderCommand
    {
        public AddToOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            ITelegramHookrRepository hookrRepository,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository,
                translationsResolver)
        {
        }

        protected override IQueryable<Order> SideQuery(IQueryable<Order> query)
            => query
                .Include(x => x.OrderedHookahs)
                .Include(x => x.OrderedTobaccos);

        protected override async Task<Order> ProcessAsync(Order order)
        {
            var (productName, productIndex, count) = ExtractProductProperties(ArgumentsLeft);
            if (count < 1)
            {
                throw new InvalidOperationException("Seems like there is no real count.");
            }

            switch (productName)
            {
                case nameof(Hookah):
                {
                    await AddProductToOrderAsync(order,
                        productIndex,
                        count,
                        context => context.Hookahs,
                        x => x.OrderedHookahs);
                    break;
                }
                case nameof(Tobacco):
                {
                    await AddProductToOrderAsync(order,
                        productIndex,
                        count,
                        context => context.Tobaccos,
                        x => x.OrderedTobaccos);
                    break;
                }
                default:
                {
                    throw new InvalidOperationException("Wrong product type " + productName);
                }
            }

            await HookrRepository.SaveChangesAsync();
            return order;
        }

        private static (string ProductName, int ProductIndex, int Count) ExtractProductProperties(IReadOnlyList<string> args)
        {
            if (args.Count != 3)
            {
                throw new InvalidArgumentsPassedInException("Wrong arguments have been passed in.");
            }

            var productName = args[0];
            if (!(int.TryParse(args[1], out var productIndex)
                  &&
                  int.TryParse(args[2], out var count)
                  &&
                  (productName.Equals(nameof(Tobacco))
                   || productName.Equals(nameof(Hookah)))
                ))
            {
                throw new InvalidArgumentsPassedInException("Wrong arguments have been passed in.");
            }

            return (
                productName,
                productIndex,
                count
            );
        }

        private async Task AddProductToOrderAsync<TProduct, TOrderedProduct>(
            Order order,
            int productIndex,
            int count,
            Func<HookrContext, DbSet<TProduct>> productTableSelector,
            Func<Order, ICollection<TOrderedProduct>?> orderedCollectionSelector)
            where TProduct : Product
            where TOrderedProduct : Ordered<TProduct>, new()
        {
            var products = await HookrRepository.ReadAsync((context, token) => productTableSelector(context)
                .ToArrayAsync(token));
            var product = products.ElementAt(productIndex - 1);
            if (product == null)
            {
                throw new InvalidOperationException($"Missing {typeof(TProduct)} with index {productIndex}");
            }

            var collection = orderedCollectionSelector(order);
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            var existingProduct = collection.FirstOrDefault(x => x.ProductId == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Count += count;
                return;
            }

            collection.Add(new TOrderedProduct
            {
                Product = product,
                Count = count
            });
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Order response)
            => await client.SendTextMessageAsync(
                await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.AddProductToOrderSuccess, response.Id));
    }
}