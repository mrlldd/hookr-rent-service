using System.Threading.Tasks;
using Hookr.Telegram.Repository.Context.Entities.Translations.Telegram;

namespace Hookr.Telegram.Utilities.Telegram.Translations
{
    public interface ITranslationsResolver
    {
        Task<string> ResolveAsync(TelegramTranslationKeys telegramTranslationKey, params object[] args);

        Task<(string First, string Second)> ResolveAsync(
            (TelegramTranslationKeys Key, object[] Args) first,
            (TelegramTranslationKeys Key, object[] Args) second
        );

        Task<(string First, string Second)> ResolveAsync(
            TelegramTranslationKeys first,
            TelegramTranslationKeys second
        );

        Task<(string First, string Second, string Third)> ResolveAsync(
            (TelegramTranslationKeys Key, object[] Args) first,
            (TelegramTranslationKeys Key, object[] Args) second,
            (TelegramTranslationKeys Key, object[] Args) third
        );

        Task<(string First, string Second, string Third, string Fourth)> ResolveAsync(
            (TelegramTranslationKeys Key, object[] Args) first,
            (TelegramTranslationKeys Key, object[] Args) second,
            (TelegramTranslationKeys Key, object[] Args) third,
            (TelegramTranslationKeys Key, object[] Args) fourth
        );

        Task<(string First, string Second, string Third, string Fourth, string Fifth)> ResolveAsync(
            (TelegramTranslationKeys Key, object[] Args) first,
            (TelegramTranslationKeys Key, object[] Args) second,
            (TelegramTranslationKeys Key, object[] Args) third,
            (TelegramTranslationKeys Key, object[] Args) fourth,
            (TelegramTranslationKeys Key, object[] Args) fifth
        );

        Task<(string First, string Second, string Third, string Fourth, string Fifth, string Sixth)>
            ResolveAsync(
                (TelegramTranslationKeys Key, object[] Args) first,
                (TelegramTranslationKeys Key, object[] Args) second,
                (TelegramTranslationKeys Key, object[] Args) third,
                (TelegramTranslationKeys Key, object[] Args) fourth,
                (TelegramTranslationKeys Key, object[] Args) fifth,
                (TelegramTranslationKeys Key, object[] Args) sixth
            );

        Task<(string First, string Second, string Third, string Fourth, string Fifth, string Sixth, string Seventh)>
            ResolveAsync(
                (TelegramTranslationKeys Key, object[] Args) first,
                (TelegramTranslationKeys Key, object[] Args) second,
                (TelegramTranslationKeys Key, object[] Args) third,
                (TelegramTranslationKeys Key, object[] Args) fourth,
                (TelegramTranslationKeys Key, object[] Args) fifth,
                (TelegramTranslationKeys Key, object[] Args) sixth,
                (TelegramTranslationKeys Key, object[] Args) seventh
            );
    }
}