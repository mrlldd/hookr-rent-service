using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Models.Telegram.Exceptions;
using Hookr.Telegram.Operations.Base;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;

namespace Hookr.Telegram.Operations.Commands.Orders
{
    public abstract class OrderCommandBase : CommandWithResponse<Order>
    {
        private const string Space = " ";

        protected readonly IUserContextProvider UserContextProvider;
        protected readonly IHookrRepository HookrRepository;
        protected List<string> ArgumentsLeft { get; } = new List<string>();
        protected OrderCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
            ITranslationsResolver translationsResolver) 
            : base(telegramBotClient,
            translationsResolver)
        {
            UserContextProvider = userContextProvider;
            HookrRepository = hookrRepository;
        }

        protected sealed override async Task<Order> ProcessAsync()
        {
            var orderId = ExtractOrderId(UserContextProvider.Update.Content);
            var order = await HookrRepository.ReadAsync((context, token) => SideQuery(context.Orders)
                .FirstOrDefaultAsync(x => x.Id == orderId, token));
            await ValidateOrderAsync(order, UserContextProvider.DatabaseUser);
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

            ArgumentsLeft.AddRange(subs
                .Skip(2));
            return int.TryParse(subs[1], out var result)
                ? result
                : throw new InvalidArgumentsPassedInException("Wrong arguments have been passed in.");
        }
        
        private async Task ValidateOrderAsync(Order order, TelegramUser user)
        {
            if (order == null)
            {
                throw new InsufficientAccessRightsException("Order not exist or you have no access.");
            }
            if (order.IsDeleted)
            {
                throw new OrderAlreadyDeletedException(await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.OrderAlreadyDeleted));
            }

            switch (user.State)
            {
                /*case TelegramUserStates.Dev:
                    return;*/
                case TelegramUserStates.Default when order.CreatedById != user.Id:
                    throw new InsufficientAccessRightsException("Seems like you have no access :(");
            }

            await CustomOrderAsyncValidator(order, user);
        }

        protected virtual Task CustomOrderAsyncValidator(Order order, TelegramUser user) => Task.CompletedTask;

        protected virtual IQueryable<Order> SideQuery(IQueryable<Order> query) => query;
    }
}