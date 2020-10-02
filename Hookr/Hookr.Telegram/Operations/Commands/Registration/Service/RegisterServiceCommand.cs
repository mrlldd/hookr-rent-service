using System;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Repository.Context.Entities.Base;
using Hookr.Telegram.Utilities.App.Settings;
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