using System;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Telegram.Config;
using Hookr.Telegram.Config.Management;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Translations;

namespace Hookr.Telegram.Operations.Commands.Registration.Service
{
    public class RegisterServiceCommand : RegisterCommandBase, IRegisterServiceCommand
    {
        protected override TelegramUserStates ExpectedState => TelegramUserStates.Service;

        protected override Func<Guid, IManagementConfig, bool> KeyValidator =>
            (key, config) => config.ServiceKey.Equals(key);

        public RegisterServiceCommand(IExtendedTelegramBotClient telegramBotClient,
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