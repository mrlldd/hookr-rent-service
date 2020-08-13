using System.Threading.Tasks;
using HookrTelegramBot.Repository.Context.Entities.Translations;

namespace HookrTelegramBot.Utilities.Telegram.Translations
{
    public interface ITranslationsResolver
    {
        Task<string> ResolveAsync(TranslationKeys translationKey, params object[] args);

        Task<(string First, string Second)> ResolveAsync(
            (TranslationKeys Key, object[] Args) first,
            (TranslationKeys Key, object[] Args) second
        );

        Task<(string First, string Second, string Third)> ResolveAsync(
            (TranslationKeys Key, object[] Args) first,
            (TranslationKeys Key, object[] Args) second,
            (TranslationKeys Key, object[] Args) third
        );

        Task<(string First, string Second, string Third, string Fourth)> ResolveAsync(
            (TranslationKeys Key, object[] Args) first,
            (TranslationKeys Key, object[] Args) second,
            (TranslationKeys Key, object[] Args) third,
            (TranslationKeys Key, object[] Args) fourth
        );

        Task<(string First, string Second, string Third, string Fourth, string Fifth)> ResolveAsync(
            (TranslationKeys Key, object[] Args) first,
            (TranslationKeys Key, object[] Args) second,
            (TranslationKeys Key, object[] Args) third,
            (TranslationKeys Key, object[] Args) fourth,
            (TranslationKeys Key, object[] Args) fifth
        );
    }
}