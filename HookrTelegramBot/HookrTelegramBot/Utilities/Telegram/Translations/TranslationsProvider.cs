using System.Threading.Tasks;
using HookrTelegramBot.Repository.Context.Entities.Translations;

namespace HookrTelegramBot.Utilities.Telegram.Translations
{
    public class TranslationsProvider : ITranslationsProvider
    {
        public Task<string> GetAsync(LanguageCodes languageCode, TranslationKeys translationKey)
        {
            throw new System.NotImplementedException();
        }
    }
}