﻿using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram.Exceptions;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders
{
    public abstract class OrderCommandBase : CommandWithResponse<Order>
    {
        private const string Space = " ";

        private readonly IUserContextProvider userContextProvider;
        protected readonly IHookrRepository HookrRepository;
        protected string[] ArgumentsLeft { get; private set; }
        protected OrderCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository) : base(telegramBotClient)
        {
            this.userContextProvider = userContextProvider;
            HookrRepository = hookrRepository;
        }

        protected sealed override async Task<Order> ProcessAsync()
        {
            var orderId = ExtractOrderId(userContextProvider.Update.Content);
            var order = await HookrRepository.ReadAsync((context, token) => SideQuery(context.Orders)
                .FirstOrDefaultAsync(x => x.Id == orderId, token));
            ValidateOrder(order, userContextProvider.DatabaseUser);
            return await ProcessAsync(order);
        }

        protected virtual Task<Order> ProcessAsync(Order order) => Task.FromResult(order);

        private int ExtractOrderId(string command)
        {
            var subs = command.Split(Space);
            if (subs.Length < 2)
            {
                throw new InvalidArgumentsPassedInException("Wrong arguments have been passed in.");
            }

            ArgumentsLeft = subs
                .Skip(2)
                .ToArray();
            return int.TryParse(subs[1], out var result)
                ? result
                : throw new InvalidArgumentsPassedInException("Wrong arguments have been passed in.");
        }
        
        private static void ValidateOrder(Order order, TelegramUser user)
        {
            if (order == null)
            {
                throw new InsufficientAccessRightsException("Order not exist or you have no access.");
            }
            if (order.IsDeleted)
            {
                throw new OrderAlreadyDeletedException("Order not exist or has been already deleted.");
            }

            switch (user.State)
            {
                case TelegramUserStates.Dev:
                    return;
                case TelegramUserStates.Default when order.CreatedById != user.Id:
                    throw new InsufficientAccessRightsException("Seems like you have no access :(");
            }
        }

        protected virtual IQueryable<Order> SideQuery(IQueryable<Order> query) => query;
    }
}