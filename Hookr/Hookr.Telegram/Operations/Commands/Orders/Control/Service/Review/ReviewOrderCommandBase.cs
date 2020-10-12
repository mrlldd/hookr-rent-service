using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context.Entities;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Notifiers;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Operations.Commands.Orders.Control.Service.Review
{
    public abstract class ReviewOrderCommandBase : OrderCommandBase
    {
        protected readonly ITelegramUsersNotifier TelegramUsersNotifier;

        protected ReviewOrderCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            ITelegramHookrRepository hookrRepository,
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