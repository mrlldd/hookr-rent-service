using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities.Translations;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;

namespace Hookr.Telegram.Repository
{
    public interface ITelegramHookrRepository : IHookrRepository
    {
        Task<string?> GetTranslationAsync(LanguageCodes languageCode,
            TelegramTranslationKeys telegramTranslationKey,
            bool omitCache = false,
            CancellationToken token = default);

        Task<IDictionary<TelegramTranslationKeys, string>> GetTranslationsAsync(LanguageCodes languageCode,
            TelegramTranslationKeys[] translationKeys,
            bool omitCache = false,
            CancellationToken token = default);
    }
}