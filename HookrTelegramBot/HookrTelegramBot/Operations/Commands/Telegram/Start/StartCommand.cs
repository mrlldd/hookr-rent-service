using System;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Translations;
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
            IUserContextProvider userContextProvider,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                translationsResolver)
        {
            this.hookrRepository = hookrRepository;
            this.userContextProvider = userContextProvider;
        }

        protected override Task ProcessAsync()
        {
            var user = userContextProvider.Update.RealMessage.From;
            var dbUser = userContextProvider.DatabaseUser;
            var now = DateTime.Now;
            if(dbUser != null)
            {
                dbUser.Username = user.Username;
                dbUser.LastUpdatedAt = now;
            }
            else
            {
                hookrRepository.Context.TelegramUsers.Add(new TelegramUser
                {
                    Id = user.Id,
                    State = TelegramUserStates.Default,
                    Username = user.Username,
                    LastUpdatedAt = now
                });
            }

            return hookrRepository.Context.SaveChangesAsync();
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => await client
                .SendTextMessageAsync(await TranslationsResolver.ResolveAsync(TranslationKeys.Welcome), replyMarkup: new ReplyKeyboardMarkup
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