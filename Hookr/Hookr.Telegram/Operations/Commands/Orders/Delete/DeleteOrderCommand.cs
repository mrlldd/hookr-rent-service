using System;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Models.Telegram.Exceptions;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Operations.Commands.Orders.Delete
{
    public class DeleteOrderCommand : OrderCommandBase, IDeleteOrderCommand
    {
        public DeleteOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository,
                translationsResolver)
        {
        }

        protected override async Task<Order> ProcessAsync(Order order)
        {
            HookrRepository.Context.Orders.Remove(order);
            await HookrRepository.SaveChangesAsync();
            return order;
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Order response)
            => await client.SendTextMessageAsync(
                await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.OrderDeleteSuccess, response.Id));

        protected override async Task<(bool, string)> ReadCustomExceptionAsync(Exception exception)
            => exception is OrderAlreadyDeletedException
                ? (true, await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.OrderAlreadyDeleted))
                : await base.ReadCustomExceptionAsync(exception);
    }
}