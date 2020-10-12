using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Telegram.Config;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Translations;

namespace Hookr.Telegram.Operations.Commands.Registration.Unregister
{
    public class UnregisterCommand : RegisterCommandBase, IUnregisterCommand
    {
        public UnregisterCommand(IExtendedTelegramBotClient telegramBotClient,
            ITelegramHookrRepository hookrRepository,
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

        protected override TelegramUserStates StateToSet
            => TelegramUserStates.Default;

        protected override bool OmitKeyValidation => true;
    }
}