using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using HookrTelegramBot.Controllers;
using HookrTelegramBot.Utilities.App.Settings;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Provider
{
    public class TelegramBotProvider : ITelegramBotProvider
    {
        private readonly IAppSettings appSettings;
        private readonly IHttpClientFactory httpClientFactory;

        public TelegramBotProvider(IAppSettings appSettings,
            IHttpClientFactory httpClientFactory)
        {
            this.appSettings = appSettings;
            this.httpClientFactory = httpClientFactory;
        }
        public ITelegramBotClient Instance { get; private set; }
        public User Info { get; private set; }

        public async Task InitializeAsync(CancellationToken token = default)
        {
            Log.Information("Initializing bot instance...");
            if (Instance != null)
            {
                throw new InvalidOperationException("Bot is already initialized.");
            }
            var bot = new TelegramBotClient(appSettings.TelegramBotToken, httpClientFactory.CreateClient());
            var info = await bot.GetMeAsync(token);
            var route = typeof(TelegramController).GetCustomAttribute<RouteAttribute>()?.Template;
            if (string.IsNullOrEmpty(route))
            {
                throw new InvalidOperationException("Webhook route is not initialized");
            }
            var finalWebhook = $"{appSettings.WebhookUrl}/{route}/update";
            await bot.SetWebhookAsync(finalWebhook, cancellationToken: token);
            Instance = bot;
            Info = info;
            Log.Information("Successfully initialized telegram bot instance (@{0}) with webhook {1}", info.Username, finalWebhook);
        }
    }
}