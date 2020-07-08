using System;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.App.Settings;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;

namespace HookrTelegramBot.Operations.Commands.Telegram.Registration.Developer
{
    public class RegisterDeveloperCommand : RegisterCommandBase, IRegisterDeveloperCommand
    {
        protected override TelegramUserStates ExpectedState => TelegramUserStates.Dev;

        protected override Func<Guid, IManagementConfig, bool> KeyValidator { get; } =
            (key, config) => config.DeveloperKey.Equals(key);

        public RegisterDeveloperCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserContextProvider userContextProvider,
            IAppSettings appSettings)
            : base(telegramBotClient, hookrRepository, userContextProvider, appSettings)
        {
        }
    }
}