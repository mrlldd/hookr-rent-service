using System;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.App.Settings;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Translations;

namespace HookrTelegramBot.Operations.Commands.Telegram.Registration.Unregister
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