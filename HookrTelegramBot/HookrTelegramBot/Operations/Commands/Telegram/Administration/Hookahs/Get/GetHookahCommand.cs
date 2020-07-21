using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Delete;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Get
{
    public class GetHookahCommand : GetSingleCommandBase<Hookah>, IGetHookahCommand
    {
        public GetHookahCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository) 
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository)
        {
        }

        protected override Identified<Hookah> CastToResult(Hookah entity, int index)
            => new Identified<Hookah>
            {
                Entity = entity,
                Index = index
            };
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
                .SendTextMessageAsync($"Here is your hookah {response.Entity.Name} - {response.Entity.Price}",
                    replyMarkup: PrepareKeyboard(response));

        private InlineKeyboardMarkup PrepareKeyboard(Identified<Hookah> hookah)
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