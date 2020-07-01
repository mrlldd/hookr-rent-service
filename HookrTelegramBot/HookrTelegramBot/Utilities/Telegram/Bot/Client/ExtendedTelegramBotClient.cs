﻿using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Bot.Provider;
using HookrTelegramBot.Utilities.Telegram.Selectors.UpdateMessage;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Client
{
    public class ExtendedTelegramBotClient : DecoratedTelegramBotClientBase, IExtendedTelegramBotClient
    {
        public ExtendedTelegramBotClient(IUpdateMessageSelector messageSelector,
            IUserContextProvider userContextProvider,
            ITelegramBotProvider provider,
            bool omitEventProxies = true)
            : base(provider, omitEventProxies)
        {
            WithCurrentUser = new CurrentTelegramUserClient(Bot, messageSelector.Select(userContextProvider.Update).Chat);
        }

        public ICurrentTelegramUserClient WithCurrentUser { get; }
    }
}