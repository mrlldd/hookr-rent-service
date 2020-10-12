using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Operations.Base;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Hookr.Telegram.Operations.Commands.Start
{
    public class StartCommand : CommandWithResponse, IStartCommand
    {
        private readonly ITelegramHookrRepository hookrRepository;
        private readonly IUserContextProvider userContextProvider;

        public StartCommand(IExtendedTelegramBotClient telegramBotClient,
            ITelegramHookrRepository hookrRepository,
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
            if (dbUser != null)
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

            return hookrRepository.SaveChangesAsync();
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => await client
                .SendTextMessageAsync(await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.Welcome),
                    replyMarkup: new ReplyKeyboardMarkup
                    {
                        OneTimeKeyboard = true,
                        ResizeKeyboard = true,
                        Keyboard = Array.Empty<IEnumerable<KeyboardButton>>()
                    });
    }
}