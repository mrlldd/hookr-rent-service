using System.Threading.Tasks;
using HookrTelegramBot.Repository.Context.Entities.Translations;

namespace HookrTelegramBot.Utilities.Telegram.Translations
{
    public interface ITranslationsProvider
    {
        Task<string> GetAsync(LanguageCodes languageCode, TranslationKeys translationKey);
    }
}