using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Resiliency;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace HookrTelegramBot.Repository
{
    public class HookrRepository : IHookrRepository
    {
        private const string TranslationCacheKeyFormat = "t_l{0}_k{1}";
        private readonly IPolicySet policySet;
        private readonly IMemoryCache memoryCache;
        public HookrContext Context { get; }

        public HookrRepository(HookrContext context,
            IPolicySet policySet,
            IMemoryCache memoryCache)
        {
            this.policySet = policySet;
            this.memoryCache = memoryCache;
            Context = context;
        }

        public Task<TResult> ReadAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor)
            => policySet.ReadPolicy.ExecuteAsync(() => functor(Context, default));

        public Task WriteAsync(Func<HookrContext, CancellationToken, Task> functor)
            => policySet.WritePolicy.ExecuteAsync(() => functor(Context, default));
        
        public Task<TResult> WriteAsync<TResult>(Func<HookrContext, CancellationToken, Task<TResult>> functor)
            => policySet.WritePolicy.ExecuteAsync(() => functor(Context, default));

        public Task<int> SaveChangesAsync()
            => WriteAsync((context, token) => context.SaveChangesAsync(token));

        public async Task<string> GetTranslationAsync(LanguageCodes languageCode,
            TranslationKeys translationKey,
            bool omitCache = false)
        {
            const double timeoutDays = 1;
            var key = string.Format(TranslationCacheKeyFormat, languageCode, translationKey);
            if (!omitCache
                && memoryCache.TryGetValue<string>(key,
                    out var result))
            {
                return result;
            }

            var translation = await policySet.ReadPolicy
                .ExecuteAsync(() => Context.Translations
                    .FirstOrDefaultAsync(x
                        => x.Language == languageCode
                           && x.Key == translationKey));
            var value = translation?.Value;
            if (!string.IsNullOrEmpty(value))
            {
                memoryCache.Set(key, value, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromDays(timeoutDays)
                });
            }

            return value;
        }

        public async Task<IDictionary<TranslationKeys, string>> GetTranslationsAsync(LanguageCodes languageCode,
            bool omitCache = false, params TranslationKeys[] translationKeys)
        {
            const double timeoutDays = 1;
            var resultDictionary = new Dictionary<TranslationKeys, string>();
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
                .ExecuteAsync(() => Context.Translations
                    .Where(x
                        => x.Language == languageCode
                           && translationsLeft.Contains(x.Key))
                    .ToDictionaryAsync(x => x.Key, x => x.Value)
                );
            translations
                .ForEach(x =>
                {
                    var (key, value) = x;
                    var cacheKey = string.Format(TranslationCacheKeyFormat, languageCode, key);
                    if (!string.IsNullOrEmpty(value))
                    {
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