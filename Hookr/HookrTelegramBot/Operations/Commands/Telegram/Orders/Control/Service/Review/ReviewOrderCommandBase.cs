using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Notifiers;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Service.Review
{
    public abstract class ReviewOrderCommandBase : OrderCommandBase
    {
        protected readonly ITelegramUsersNotifier TelegramUsersNotifier;

        protected ReviewOrderCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
            ITranslationsResolver translationsResolver,
            ITelegramUsersNotifier telegramUsersNotifier)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository,
                translationsResolver)
        {
            TelegramUsersNotifier = telegramUsersNotifier;
        }

        protected abstract OrderStates AllowedState { get; }
        protected abstract OrderStates NextOrderState { get; }

        protected sealed override async Task<Order> ProcessAsync(Order order)
        {
            order.State = NextOrderState;
            await HookrRepository.SaveChangesAsync();
            await NotifyAsync(order, UserContextProvider.DatabaseUser);
            return order;
        }

        protected override Task CustomOrderAsyncValidator(Order order, TelegramUser user)
        {
            if (order.State != AllowedState)
            {
                throw new InvalidOperationException("Order has insufficient state to perform this action.");
            }

            return Task.CompletedTask;
        }

        protected abstract Task<IEnumerable<Message>> NotifyAsync(Order order, TelegramUser user);
    }
}