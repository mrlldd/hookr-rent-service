using System;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Config;
using Hookr.Telegram.Config.Management;
using Hookr.Telegram.Operations.Base;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Operations.Commands.Registration
{
    public abstract class RegisterCommandBase : CommandWithResponse
    {
        private const string Space = " ";
        protected abstract TelegramUserStates StateToSet { get; }


        private readonly ITelegramHookrRepository hookrRepository;
        private readonly IUserContextProvider userContextProvider;
        private readonly IApplicationConfig applicationConfig;
        private readonly DateTime now = DateTime.Now;

        protected RegisterCommandBase(IExtendedTelegramBotClient telegramBotClient,
            ITelegramHookrRepository hookrRepository,
            IUserContextProvider userContextProvider,
            IApplicationConfig applicationConfig,
            ITranslationsResolver translationsResolver) :
            base(telegramBotClient,
                translationsResolver)
        {
            this.hookrRepository = hookrRepository;
            this.userContextProvider = userContextProvider;
            this.applicationConfig = applicationConfig;
        }

        protected override async Task ProcessAsync()
        {
            if (!OmitKeyValidation)
            {
                var (key, keyExtractSuccess) = ExtractKey(userContextProvider.Update.RealMessage.Text);
                if (!keyExtractSuccess || !KeyValidator(key, applicationConfig.Management))
                {
                    throw new InvalidOperationException("Wrong arguments for registration.");
                }
            }

            var user = TelegramBotClient.WithCurrentUser.User;
            if (await hookrRepository
                .ReadAsync((context, token) =>
                    context.TelegramUsers
                        .AnyAsync(x => x.Id == user.Id, token)))
            {
                var dbUser = userContextProvider.DatabaseUser;
                dbUser.State = StateToSet;
                dbUser.Username = user.Username;
                dbUser.LastUpdatedAt = now;
            }
            else
            {
                hookrRepository.Context.TelegramUsers.Add(new TelegramUser
                {
                    Id = user.Id,
                    State = StateToSet,
                    Username = user.Username,
                    LastUpdatedAt = now
                });
            }

            await hookrRepository.SaveChangesAsync();
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => await client
                .SendTextMessageAsync(
                    await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.UserStateRegistrationSuccess,
                        StateToSet.ToString().ToLower())
                );

        protected override Task<Message> SendErrorAsync(ICurrentTelegramUserClient client, Exception exception)
            => exception is AggregateException aggregated && aggregated.InnerException is InvalidOperationException
                ? client.SendTextMessageAsync("Seems like there is a wrong key passed in.")
                : base.SendErrorAsync(client, exception);

        private static (Guid Key, bool Success) ExtractKey(string messageWithCommand)
        {
            var subs = messageWithCommand
                .Split(Space);
            if (subs.Length != 2)
            {
                return (Guid.Empty, false);
            }

            var success = Guid.TryParse(subs[1], out var key);
            return (key, success);
        }

        protected virtual bool OmitKeyValidation { get; } = false;

        protected virtual bool KeyValidator(Guid key, IManagementConfig config) => false;
    }
}