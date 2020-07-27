﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Delete;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Delete;
using HookrTelegramBot.Operations.Commands.Telegram.Orders;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Caches.CurrentOrder;
using HookrTelegramBot.Utilities.Telegram.Caches.UserTemporaryStatus;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Get
{
    public class GetTobaccoCommand : GetSingleCommandBase<Tobacco>, IGetTobaccoCommand
    {
        private readonly ICurrentOrderCache currentOrderCache;

        public GetTobaccoCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
            ICurrentOrderCache currentOrderCache)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository)
        {
            this.currentOrderCache = currentOrderCache;
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client,
            Identified<Tobacco> response)
            => client
                .SendTextMessageAsync($"Here is your hookah {response.Entity.Name} - {response.Entity.Price}",
                    replyMarkup: PrepareKeyboard(response));

        protected override DbSet<Tobacco> EntityTableSelector(HookrContext context)
            => context.Tobaccos;

        protected override int ExtractIndex(string command)
            => int
                .TryParse(command
                    .Trim()
                    .Substring(1), out var result)
                ? result
                : throw new InvalidOperationException("Wrong arguments.");

        private InlineKeyboardMarkup PrepareKeyboard(Identified<Tobacco> tobacco)
        {
            var buttons = new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton
                {
                    Text = "Order",
                    CallbackData = "/start"
                }
            };
            var dbUser = UserContextProvider.DatabaseUser;
            var orderId = currentOrderCache.Get(dbUser.Id);
            if (orderId.HasValue)
            {
                buttons.Add(new InlineKeyboardButton
                {
                    Text = "Add to order",
                    CallbackData = $"/{nameof(AddToOrderCommand).ExtractCommandName()} {orderId} {typeof(Tobacco).Name} {tobacco.Index}"
                });
            }

            if (dbUser.State > TelegramUserStates.Default)
            {
                buttons.AddRange(new[]
                {
                    new InlineKeyboardButton
                    {
                        Text = "Delete",
                        CallbackData = $"/{nameof(DeleteTobaccoCommand).ExtractCommandName()} {tobacco.Index}"
                    },
                });
            }

            return new InlineKeyboardMarkup(buttons);
        }
    }
}