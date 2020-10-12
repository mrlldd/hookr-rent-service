using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Telegram.Config;
using Hookr.Telegram.Config.Telegram;
using Hookr.Telegram.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Hookr.Telegram.Utilities.Telegram.Bot.Provider
{
    public class TelegramBotProvider : ITelegramBotProvider
    {
        private readonly IExtendedTelegramConfig extendedTelegramConfig;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<TelegramBotProvider> logger;

        public TelegramBotProvider(IExtendedTelegramConfig extendedTelegramConfig,
            IHttpClientFactory httpClientFactory,
            ILogger<TelegramBotProvider> logger)
        {
            this.extendedTelegramConfig = extendedTelegramConfig;
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        [NotNull]
        public ITelegramBotClient? Instance { get; private set; }
        [NotNull]
        public User? Info { get; private set; }

        public async Task InitializeAsync(CancellationToken token = default)
        {
            Log.Information("Initializing bot client...");
            if (Instance != null)
            {
                throw new InvalidOperationException("Bot is already initialized.");
            }

            var bot = new TelegramBotClient(extendedTelegramConfig.Token, httpClientFactory.CreateClient());
            var info = await bot.GetMeAsync(token);
            var route = typeof(TelegramController).GetCustomAttribute<RouteAttribute>()?.Template;
            if (string.IsNullOrEmpty(route))
            {
                throw new InvalidOperationException("Webhook route is not initialized");
            }

            var finalWebhook = $"{extendedTelegramConfig.Webhook}/{route}/update";
            await bot.SetWebhookAsync(finalWebhook,
                cancellationToken: token,
                allowedUpdates: new[]
                {
                    UpdateType.Message,
                    UpdateType.CallbackQuery
                });
            Instance = bot;
            Info = info;
            logger.LogInformation("Successfully initialized telegram bot instance (@{0}) with webhook {1}", info.Username,
                finalWebhook);
        }
    }
}