using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Delete;
using HookrTelegramBot.Repository;
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
    public class GetHookahCommand : CommandWithResponse<GetHookahCommand.IdentifiedHookah>, IGetHookahCommand
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IHookrRepository hookrRepository;

        public GetHookahCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository) : base(telegramBotClient)
        {
            this.userContextProvider = userContextProvider;
            this.hookrRepository = hookrRepository;
        }

        protected override async Task<IdentifiedHookah> ProcessAsync()
        {
            var id = ExtractArguments(userContextProvider.Update.RealMessage.Text);
            var hookahs = await hookrRepository.ReadAsync((context, token)
                => context.Hookahs.ToArrayAsync(token));
            return new IdentifiedHookah
            {
                Hookah = hookahs.ElementAt(id - 1),
                Index = id
            };
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, IdentifiedHookah response)
            => client
                .SendTextMessageAsync($"Here is your hookah {response.Hookah.Name} - {response.Hookah.Price}",
                    replyMarkup: PrepareKeyboard(response));

        private InlineKeyboardMarkup PrepareKeyboard(IdentifiedHookah hookah)
        {
            var buttons = new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton
                {
                    Text = "Order",
                    CallbackData = "/start"
                }
            };
            var dbUser = userContextProvider.DatabaseUser;
            if (dbUser.State > TelegramUserStates.Default)
            {
                buttons.AddRange(new[]
                {
                    new InlineKeyboardButton
                    {
                        Text = "Delete",
                        CallbackData = $"/{nameof(DeleteHookahCommand).ExtractCommandName()} {hookah.Index}"
                    },
                    new InlineKeyboardButton
                    {
                        Text = "Edit",
                        CallbackData = "/start"
                    }, 
                });
            }

            return new InlineKeyboardMarkup(buttons);
        }

        private static int ExtractArguments(string messageText)
            => int
                .Parse(messageText
                    .Trim()
                    .Substring(1));

        public class IdentifiedHookah
        {
            public Hookah Hookah { get; set; }
            public int Index { get; set; }
        }
    }
}