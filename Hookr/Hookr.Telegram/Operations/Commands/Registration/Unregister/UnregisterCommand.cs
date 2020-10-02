using Hookr.Telegram.Repository;
using Hookr.Telegram.Repository.Context.Entities.Base;
using Hookr.Telegram.Utilities.App.Settings;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Translations;

namespace Hookr.Telegram.Operations.Commands.Registration.Unregister
{
    public class UnregisterCommand : RegisterCommandBase, IUnregisterCommand
    {
        public UnregisterCommand(IExtendedTelegramBotClient telegramBotClient,
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

        protected override TelegramUserStates ExpectedState
            => TelegramUserStates.Default;

        protected override bool OmitKeyValidation => true;
    }
}