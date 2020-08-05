using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
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
                    var hookahs = await hookrRepository.ReadAsync((context, token) => context.Hookahs
                        .ToArrayAsync(token));
                    var hookah = hookahs.ElementAt(productIndex);
                    if (hookah == null)
                    {
                        throw new InvalidOperationException($"Missing hookah with index {productIndex}");
                    }

                    order.OrderedHookahs.Add(new OrderedHookah
                    {
                        Product = hookah
                    });
                    break;
                }
                case nameof(Tobacco):
                {
                    var tobaccos = await hookrRepository.ReadAsync((context, token) => context.Tobaccos
                        .ToArrayAsync(token));
                    var tobacco = tobaccos.ElementAt(productIndex);
                    if (tobacco == null)
                    {
                        throw new InvalidOperationException($"Missing tobacco with index {productIndex}");
                    }

                    order.OrderedTobaccos.Add(new OrderedTobacco
                    {
                        Product = tobacco
                    });
                    break;
                }
                default:
                {
                    throw new InvalidOperationException("Wrong product type " + productName);
                }
                // todo refactor into generic method executable
            }

            await hookrRepository.Context.SaveChangesAsync();
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