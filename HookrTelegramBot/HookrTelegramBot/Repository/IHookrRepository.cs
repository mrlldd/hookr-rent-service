using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities.Translations;

namespace HookrTelegramBot.Repository
{
    public interface IHookrRepository
    {
        HookrContext Context { get; }
        Task<TResult> ReadAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor);
        Task WriteAsync(Func<HookrContext, CancellationToken, Task> functor);

        Task<string> GetTranslationAsync(LanguageCodes languageCode,
            TranslationKeys translationKey,
            bool omitCache = false);

        Task<IDictionary<TranslationKeys, string>> GetTranslationsAsync(LanguageCodes languageCode,
            bool omitCache = false,
            params TranslationKeys[] translationKeys);
    }
}