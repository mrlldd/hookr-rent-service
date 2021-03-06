﻿using System;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Telegram.Config;
using Hookr.Telegram.Config.Management;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Translations;

namespace Hookr.Telegram.Operations.Commands.Registration.Service
{
    public class RegisterServiceCommand : RegisterCommandBase, IRegisterServiceCommand
    {
        protected override TelegramUserStates StateToSet => TelegramUserStates.Service;

        public RegisterServiceCommand(IExtendedTelegramBotClient telegramBotClient,
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

        protected override bool KeyValidator(Guid key, IManagementConfig config) 
            => config.ServiceKey.Equals(key);
    }
}