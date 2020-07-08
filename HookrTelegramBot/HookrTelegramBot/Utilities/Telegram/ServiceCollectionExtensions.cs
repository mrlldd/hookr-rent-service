﻿using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Selectors;
using Microsoft.Extensions.DependencyInjection;

namespace HookrTelegramBot.Utilities.Telegram
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramServices(this IServiceCollection services)
            => services
                .AddTelegramSelectors()
                .AddTelegramBotServices();

    }
}