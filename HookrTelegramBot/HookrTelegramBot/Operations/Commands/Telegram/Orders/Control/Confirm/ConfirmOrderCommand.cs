using System;
using System.Threading.Tasks;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Notifiers;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Confirm
{
    public class ConfirmOrderCommand : OrderCommandBase, IConfirmOrderCommand
    {
        private readonly ITelegramUsersNotifier telegramUsersNotifier;

        public ConfirmOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
            ITelegramUsersNotifier telegramUsersNotifier,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository,
                translationsResolver)
        {
            this.telegramUsersNotifier = telegramUsersNotifier;
        }

        protected override async Task<Order> ProcessAsync(Order order)
        {
            if (order.State != OrderStates.Constructing)
            {
                throw new InvalidOperationException("Order is already confirmed.");
            }

            order.State = OrderStates.Confirmed;
            //todo notify service users about new order;
            await HookrRepository.Context.SaveChangesAsync();
            await telegramUsersNotifier.SendAsync((client, user) =>
                    client.SendTextMessageAsync(user.Id, "New order:\n" +
                                                         $"Id: {order.Id}"),
                TelegramUserStates.Service,
                TelegramUserStates.Dev);
            return order;
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Order response)
            => await client.SendTextMessageAsync(await TranslationsResolver.ResolveAsync(TranslationKeys.ConfirmOrderSuccess, response.Id));
    }
}