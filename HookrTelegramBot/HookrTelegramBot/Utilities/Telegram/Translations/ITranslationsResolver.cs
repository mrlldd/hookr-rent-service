﻿using System.Threading.Tasks;
using HookrTelegramBot.Repository.Context.Entities.Translations;

namespace HookrTelegramBot.Utilities.Telegram.Translations
{
    public interface ITranslationsResolver
    {
        Task<string> GetAsync(TranslationKeys translationKey);
    }
}