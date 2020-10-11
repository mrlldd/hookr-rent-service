using System.Collections.Generic;
using Hookr.Core.Utilities.Providers;

namespace Hookr.Core.Utilities.Caching
{
    public abstract class UserLevelCache<T> : Cache<T>
    {
        // ReSharper disable once MemberCanBePrivate.Global
        protected readonly ITelegramUserIdProvider TelegramUserIdProvider;

        protected UserLevelCache(ITelegramUserIdProvider telegramUserIdProvider)
        {
            TelegramUserIdProvider = telegramUserIdProvider;
        }

        protected sealed override IEnumerable<object> CacheKeyPrefixesFactory()
            => new List<object>
            {
                "user",
                TelegramUserIdProvider.ProvidedValue
            };
    }
}