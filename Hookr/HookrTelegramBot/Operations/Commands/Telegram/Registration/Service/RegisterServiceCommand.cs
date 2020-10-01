using System;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.App.Settings;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Translations;

namespace HookrTelegramBot.Operations.Commands.Telegram.Registration.Service
{
    public class RegisterServiceCommand : RegisterCommandBase, IRegisterServiceCommand
    {
        protected override TelegramUserStates ExpectedState => TelegramUserStates.Service;

        protected override Func<Guid, IManagementConfig, bool> KeyValidator { get; } =
            (key, config) => config.ServiceKey.Equals(key);

        public RegisterServiceCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserContextProvider userContextProvider,
            IAppSettings appSettings,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                hookrRepository,
                userContextProvider, 
                appSettings,
                translationsResolver)
        {
        }
    }
}