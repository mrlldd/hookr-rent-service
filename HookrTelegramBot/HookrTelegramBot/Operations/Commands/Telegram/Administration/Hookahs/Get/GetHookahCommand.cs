using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Delete;
using HookrTelegramBot.Operations.Commands.Telegram.Orders;
using HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.AddProduct;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Caches.CurrentOrder;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Get
{
    public class GetHookahCommand : GetSingleCommandBase<Hookah>, IGetHookahCommand
    {
        private readonly ICurrentOrderCache currentOrderCache;

        public GetHookahCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
            ICurrentOrderCache currentOrderCache) 
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository)
        {
            this.currentOrderCache = currentOrderCache;
        }
        
        protected override int ExtractIndex(string command)
            => int
                .TryParse(command
                    .Trim()
                    .Substring(1), out var result)
                ? result
                : throw new InvalidOperationException("Wrong arguments.");

        protected override DbSet<Hookah> EntityTableSelector(HookrContext context)
            => context.Hookahs;

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Identified<Hookah> response)
            => client
                .SendTextMessageAsync($"Here is your hookah {response.Entity.Name} - {response.Entity.Price} UAH per 1pcs",
                    replyMarkup: PrepareKeyboard(response));

        private InlineKeyboardMarkup PrepareKeyboard(Identified<Hookah> hookah)
        {
            const byte defaultCount = 1;
            var buttons = new List<InlineKeyboardButton>();
            var dbUser = UserContextProvider.DatabaseUser;
            var orderId = currentOrderCache.Get(dbUser.Id);
            if (orderId.HasValue)
            {
                buttons.Add(new InlineKeyboardButton
                {
                    Text = "Order",
                    CallbackData = $"/{nameof(AddToOrderCommand).ExtractCommandName()} {orderId} {nameof(Hookah)} {hookah.Index} {defaultCount}"  
                });
            }
            if (dbUser.State > TelegramUserStates.Default)
            {
                buttons.AddRange(new[]
                {
                    new InlineKeyboardButton
                    {
                        Text = "Delete",
                        CallbackData = $"/{nameof(DeleteHookahCommand).ExtractCommandName()} {hookah.Index}"
                    },
                });
            }

            return new InlineKeyboardMarkup(buttons);
        }

    }
}