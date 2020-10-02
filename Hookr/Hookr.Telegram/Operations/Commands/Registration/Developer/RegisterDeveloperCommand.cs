using System;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Repository.Context.Entities.Base;
using Hookr.Telegram.Utilities.App.Settings;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Translations;

namespace Hookr.Telegram.Operations.Commands.Registration.Developer
{
    public class RegisterDeveloperCommand : RegisterCommandBase, IRegisterDeveloperCommand
    {
        protected override TelegramUserStates ExpectedState => TelegramUserStates.Dev;

        protected override Func<Guid, IManagementConfig, bool> KeyValidator =>
            (key, config) => config.DeveloperKey.Equals(key);

        public RegisterDeveloperCommand(IExtendedTelegramBotClient telegramBotClient,
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