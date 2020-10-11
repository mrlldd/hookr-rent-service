using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context;
using Hookr.Core.Repository.Context.Entities.Translations;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Core.Utilities.Extensions;
using Hookr.Core.Utilities.Loaders;
using Hookr.Core.Utilities.Providers;
using Hookr.Core.Utilities.Resiliency;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace Hookr.Telegram.Repository
{
    public class TelegramHookrRepository : HookrRepository, ITelegramHookrRepository
    {
        private readonly IMemoryCache memoryCache;
        private const string TranslationCacheKeyFormat = "app:translations:{0}:{1}";

        public TelegramHookrRepository(HookrContext context,
            IPolicySet policySet,
            ITelegramUserIdProvider telegramUserIdProvider,
            IMemoryCache memoryCache,
            LoaderDispatcher loaderDispatcher)
            : base(context,
                policySet,
                telegramUserIdProvider,
                loaderDispatcher)
        {
            this.memoryCache = memoryCache;
        }

        public async Task<string?> GetTranslationAsync(LanguageCodes languageCode,
            TelegramTranslationKeys telegramTranslationKey,
            bool omitCache = false,
            CancellationToken token = default)
        {
            const double timeoutDays = 1;
            var key = string.Format(TranslationCacheKeyFormat, languageCode, telegramTranslationKey);
            if (memoryCache.TryGetValue<string>(key,
                out var result))
            {
                return result;
            }

            var translation = await PolicySet.ReadPolicy
                .ExecuteAsync(() => Context.TelegramTranslations
                    .FirstOrDefaultAsync(x
                        => x.Language == languageCode
                           && x.Key == telegramTranslationKey, token));
            var value = translation.Value;
            if (!string.IsNullOrEmpty(value))
            {
                memoryCache.Set(key, value, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromDays(timeoutDays)
                });
            }

            return value;
        }

        public async Task<IDictionary<TelegramTranslationKeys, string>> GetTranslationsAsync(LanguageCodes languageCode,
            TelegramTranslationKeys[] translationKeys,
            bool omitCache = false,
            CancellationToken token = default)
        {
            const double timeoutDays = 1;
            var resultDictionary = new Dictionary<TelegramTranslationKeys, string>();
            if (!omitCache)
            {
                translationKeys
                    .ForEach(x =>
                    {
                        var key = string.Format(TranslationCacheKeyFormat, languageCode, x);
                        if (memoryCache.TryGetValue<string>(key,
                            out var translation))
                        {
                            resultDictionary.Add(x, translation);
                        }
                    });
            }

            var translationsLeft = translationKeys
                .Except(resultDictionary.Keys)
                .ToArray();
            if (!translationsLeft.Any())
            {
                return resultDictionary;
            }

            var translations = await PolicySet.ReadPolicy
                .ExecuteAsync(() => Context.TelegramTranslations
                    .Where(x
                        => x.Language == languageCode
                           && translationsLeft.Contains(x.Key))
                    .ToDictionaryAsync(x => x.Key, x => x.Value, token)
                );
            translations
                .ForEach(x =>
                {
                    var (key, value) = x;
                    if (!string.IsNullOrEmpty(value))
                    {
                        var cacheKey = string.Format(TranslationCacheKeyFormat, languageCode, key);
                        memoryCache.Set(cacheKey, value, new MemoryCacheEntryOptions
                        {
                            SlidingExpiration = TimeSpan.FromDays(timeoutDays)
                        });
                    }

                    resultDictionary.Add(x.Key, x.Value);
                });
            return resultDictionary;
        }
    }
}