using System;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Telegram.Config;
using Hookr.Telegram.Config.Management;
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
            IApplicationConfig applicationConfig,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                hookrRepository,
                userContextProvider,
                applicationConfig,
                translationsResolver)
        {
        }
    }
}