using System;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.App.Settings;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;

namespace HookrTelegramBot.Operations.Commands.Telegram.Registration.Unregister
{
    public class UnregisterCommand : RegisterCommandBase, IUnregisterCommand
    {
        public UnregisterCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserContextProvider userContextProvider,
            IAppSettings appSettings)
            : base(telegramBotClient,
                hookrRepository,
                userContextProvider,
                appSettings)
        {
        }

        protected override TelegramUserStates ExpectedState
            => TelegramUserStates.Default;

        protected override bool OmitKeyValidation => true;
    }
}