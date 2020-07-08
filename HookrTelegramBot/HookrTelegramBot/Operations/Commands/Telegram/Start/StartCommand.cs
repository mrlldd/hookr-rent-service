using System;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HookrTelegramBot.Operations.Commands.Telegram.Start
{
    public class StartCommand : CommandWithResponse, IStartCommand
    {
        private readonly IHookrRepository hookrRepository;
        private readonly IUserContextProvider userContextProvider;

        public StartCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserContextProvider userContextProvider)
            : base(telegramBotClient)
        {
            this.hookrRepository = hookrRepository;
            this.userContextProvider = userContextProvider;
        }

        protected override async Task ProcessAsync()
        {
            var user = userContextProvider.Update.RealMessage.From;
            var dbUser = await hookrRepository.ReadAsync((context, token)
                => context.TelegramUsers.FirstOrDefaultAsync(x => x.Id == user.Id, token));
            var now = DateTime.Now;
            if(dbUser != null)
            {
                hookrRepository.Context.TelegramUsers.Update(new TelegramUser
                {
                    Id = user.Id,
                    State = dbUser.State,
                    Username = user.Username,
                    LastUpdatedAt = now
                });
            }
            else
            {
                await hookrRepository.Context.TelegramUsers.AddAsync(new TelegramUser
                {
                    Id = user.Id,
                    State = TelegramUserStates.Default,
                    Username = user.Username,
                    LastUpdatedAt = now
                });
            }

            await hookrRepository.Context.SaveChangesAsync();
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => client
                .SendTextMessageAsync("Hey!", replyMarkup: new ReplyKeyboardMarkup
                {
                    OneTimeKeyboard = true,
                    ResizeKeyboard = true,
                    Keyboard = new[]
                    {
                        new[]
                        {
                            new KeyboardButton
                            {
                                Text = "hi"
                            },
                            new KeyboardButton
                            {
                                Text = "Order some"
                            },
                        }
                    }
                });
    }
}