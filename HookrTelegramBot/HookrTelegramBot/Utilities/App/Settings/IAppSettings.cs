﻿namespace HookrTelegramBot.Utilities.App.Settings
{
    public interface IAppSettings
    {
        string TelegramBotToken { get; }
        string WebhookUrl { get; }
    }
}