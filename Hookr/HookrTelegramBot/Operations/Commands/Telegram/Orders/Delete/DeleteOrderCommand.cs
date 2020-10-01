using System;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram.Exceptions;
using HookrTelegramBot.Models.Telegram.Exceptions.Base;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders.Delete
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
            await HookrRepository.Context.SaveChangesAsync();
            return order;
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Order response)
            => await client.SendTextMessageAsync(
                await TranslationsResolver.ResolveAsync(TranslationKeys.OrderDeleteSuccess, response.Id));

        protected override async Task<(bool, string)> ReadCustomExceptionAsync(Exception exception)
            => exception is OrderAlreadyDeletedException
                ? (true, await TranslationsResolver.ResolveAsync(TranslationKeys.OrderAlreadyDeleted))
                : await base.ReadCustomExceptionAsync(exception);
    }
}