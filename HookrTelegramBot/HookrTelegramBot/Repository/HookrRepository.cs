using System;
using System.Threading;
using System.Threading.Tasks;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Resiliency;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace HookrTelegramBot.Repository
{
    public class HookrRepository : IHookrRepository
    {
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

        public async Task<string> GetTranslationAsync(LanguageCodes languageCode,
            TranslationKeys translationKey,
            bool omitCache = false)
        {
            const string keyFormat = "t_l{0}_k{1}";
            const double timeoutDays = 1;
            var key = string.Format(keyFormat, languageCode, translationKey);
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
    }
}