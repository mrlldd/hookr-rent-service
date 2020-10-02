using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Telegram.Repository.Context;
using Hookr.Telegram.Repository.Context.Entities.Translations;
using Hookr.Telegram.Repository.Context.Entities.Translations.Telegram;

namespace Hookr.Telegram.Repository
{
    public interface IHookrRepository
    {
        HookrContext Context { get; }
        Task<TResult> ReadAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor);
        Task WriteAsync(Func<HookrContext, CancellationToken, Task> functor);

        Task<TResult> WriteAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor);

        Task<int> SaveChangesAsync();

        Task<string?> GetTranslationAsync(LanguageCodes languageCode,
            TelegramTranslationKeys telegramTranslationKey,
            bool omitCache = false);

        Task<IDictionary<TelegramTranslationKeys, string>> GetTranslationsAsync(LanguageCodes languageCode,
            bool omitCache = false,
            params TelegramTranslationKeys[] translationKeys);
    }
}