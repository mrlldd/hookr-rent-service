using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context;
using Hookr.Core.Repository.Context.Entities.Translations;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Core.Utilities.Extensions;
using Hookr.Core.Utilities.Loaders;
using Hookr.Core.Utilities.Providers;
using Hookr.Core.Utilities.Resiliency;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace Hookr.Core.Repository
{
    public class HookrRepository : IHookrRepository
    {
        private const string TranslationCacheKeyFormat = "t_l{0}_k{1}";
        private readonly IPolicySet policySet;
        private readonly IMemoryCache memoryCache;
        private readonly ITelegramUserIdProvider telegramUserIdProvider;
        private readonly CachingLoaderDispatcher cachingLoaderDispatcher;
        public HookrContext Context { get; }

        public HookrRepository(HookrContext context,
            IPolicySet policySet,
            IMemoryCache memoryCache,
            ITelegramUserIdProvider telegramUserIdProvider,
            CachingLoaderDispatcher cachingLoaderDispatcher)
        {
            this.policySet = policySet;
            this.memoryCache = memoryCache;
            this.telegramUserIdProvider = telegramUserIdProvider;
            this.cachingLoaderDispatcher = cachingLoaderDispatcher;
            Context = context;
        }

        public Task<TResult> ReadAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor)
            => policySet.ReadPolicy.ExecuteAsync(() => functor(Context, default));

        public Task WriteAsync(Func<HookrContext, CancellationToken, Task> functor)
            => policySet.WritePolicy.ExecuteAsync(() => functor(Context, default));

        public Task<TResult> WriteAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor)
            => policySet.WritePolicy.ExecuteAsync(() => functor(Context, default));

        public Task<int> SaveChangesAsync()
            => WriteAsync((context, token) => context.SaveChangesAsync(telegramUserIdProvider, cachingLoaderDispatcher, token));

        public async Task<string?> GetTranslationAsync(LanguageCodes languageCode,
            TelegramTranslationKeys telegramTranslationKey,
            bool omitCache = false)
        {
            const double timeoutDays = 1;
            var key = string.Format(TranslationCacheKeyFormat, languageCode, telegramTranslationKey);
            if (!omitCache
                && memoryCache.TryGetValue<string>(key,
                    out var result))
            {
                return result;
            }

            var translation = await policySet.ReadPolicy
                .ExecuteAsync(() => Context.TelegramTranslations
                    .FirstOrDefaultAsync(x
                        => x.Language == languageCode
                           && x.Key == telegramTranslationKey));
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
            bool omitCache = false, params TelegramTranslationKeys[] translationKeys)
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

            var translations = await policySet.ReadPolicy
                .ExecuteAsync(() => Context.TelegramTranslations
                    .Where(x
                        => x.Language == languageCode
                           && translationsLeft.Contains(x.Key))
                    .ToDictionaryAsync(x => x.Key, x => x.Value)
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