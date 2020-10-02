using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hookr.Telegram.Models.Telegram;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Repository.Context.Entities.Translations;
using Hookr.Telegram.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Hookr.Telegram.Utilities.Telegram.Translations
{
    public class TranslationsResolver : ITranslationsResolver
    {
        private readonly IHookrRepository hookrRepository;
        private readonly IUserContextProvider userContextProvider;
        private const LanguageCodes DefaultLanguage = LanguageCodes.Ru;
        private const string NotFoundFormat = "[{0}] [{1}]";

        public TranslationsResolver(IHookrRepository hookrRepository,
            IUserContextProvider userContextProvider)
        {
            this.hookrRepository = hookrRepository;
            this.userContextProvider = userContextProvider;
        }

        public async Task<string> ResolveAsync(TelegramTranslationKeys telegramTranslationKey, params object[] args)
        {
            var update = userContextProvider.Update;
            var languageCode = ResolveLanguage(update);
            var result = await hookrRepository.GetTranslationAsync(languageCode, telegramTranslationKey);
            return FormatResult(result, telegramTranslationKey, args);
        }

        public async Task<(string First, string Second)> ResolveAsync((TelegramTranslationKeys Key, object[] Args) first,
            (TelegramTranslationKeys Key, object[] Args) second)
        {
            var update = userContextProvider.Update;
            var languageCode = ResolveLanguage(update);
            var result = await hookrRepository.GetTranslationsAsync(languageCode, false, first.Key, second.Key);
            return (
                FormatResult(ExtractResult(result, first.Key), first.Key, first.Args),
                FormatResult(ExtractResult(result, second.Key), second.Key, second.Args)
            );
        }

        public Task<(string First, string Second)> ResolveAsync(TelegramTranslationKeys first, TelegramTranslationKeys second)
            => ResolveAsync(
                (first, Array.Empty<object>()),
                (second, Array.Empty<object>())
            );

        public async Task<(string First, string Second, string Third)> ResolveAsync(
            (TelegramTranslationKeys Key, object[] Args) first,
            (TelegramTranslationKeys Key, object[] Args) second,
            (TelegramTranslationKeys Key, object[] Args) third)
        {
            var update = userContextProvider.Update;
            var languageCode = ResolveLanguage(update);
            var result = await hookrRepository.GetTranslationsAsync(languageCode, false,
                first.Key,
                second.Key,
                third.Key);
            return (
                FormatResult(ExtractResult(result, first.Key), first.Key, first.Args),
                FormatResult(ExtractResult(result, second.Key), second.Key, second.Args),
                FormatResult(ExtractResult(result, third.Key), third.Key, third.Args)
            );
        }

        public async Task<(string First, string Second, string Third, string Fourth)> ResolveAsync(
            (TelegramTranslationKeys Key, object[] Args) first,
            (TelegramTranslationKeys Key, object[] Args) second,
            (TelegramTranslationKeys Key, object[] Args) third,
            (TelegramTranslationKeys Key, object[] Args) fourth)
        {
            var update = userContextProvider.Update;
            var languageCode = ResolveLanguage(update);
            var result = await hookrRepository.GetTranslationsAsync(languageCode, false,
                first.Key,
                second.Key,
                third.Key,
                fourth.Key);
            return (
                FormatResult(ExtractResult(result, first.Key), first.Key, first.Args),
                FormatResult(ExtractResult(result, second.Key), second.Key, second.Args),
                FormatResult(ExtractResult(result, third.Key), third.Key, third.Args),
                FormatResult(ExtractResult(result, fourth.Key), fourth.Key, fourth.Args)
            );
        }

        public async Task<(string First, string Second, string Third, string Fourth, string Fifth)> ResolveAsync(
            (TelegramTranslationKeys Key, object[] Args) first,
            (TelegramTranslationKeys Key, object[] Args) second,
            (TelegramTranslationKeys Key, object[] Args) third,
            (TelegramTranslationKeys Key, object[] Args) fourth,
            (TelegramTranslationKeys Key, object[] Args) fifth)
        {
            var update = userContextProvider.Update;
            var languageCode = ResolveLanguage(update);
            var result = await hookrRepository.GetTranslationsAsync(languageCode, false,
                first.Key,
                second.Key,
                third.Key,
                fourth.Key,
                fifth.Key);
            return (
                FormatResult(ExtractResult(result, first.Key), first.Key, first.Args),
                FormatResult(ExtractResult(result, second.Key), second.Key, second.Args),
                FormatResult(ExtractResult(result, third.Key), third.Key, third.Args),
                FormatResult(ExtractResult(result, fourth.Key), fourth.Key, fourth.Args),
                FormatResult(ExtractResult(result, fifth.Key), fifth.Key, fifth.Args)
            );
        }

        public async Task<(string First, string Second, string Third, string Fourth, string Fifth, string Sixth)>
            ResolveAsync(
                (TelegramTranslationKeys Key, object[] Args) first,
                (TelegramTranslationKeys Key, object[] Args) second,
                (TelegramTranslationKeys Key, object[] Args) third,
                (TelegramTranslationKeys Key, object[] Args) fourth,
                (TelegramTranslationKeys Key, object[] Args) fifth,
                (TelegramTranslationKeys Key, object[] Args) sixth
                )
        {
            var update = userContextProvider.Update;
            var languageCode = ResolveLanguage(update);
            var result = await hookrRepository.GetTranslationsAsync(languageCode, false,
                first.Key,
                second.Key,
                third.Key,
                fourth.Key,
                fifth.Key,
                sixth.Key);
            return (
                FormatResult(ExtractResult(result, first.Key), first.Key, first.Args),
                FormatResult(ExtractResult(result, second.Key), second.Key, second.Args),
                FormatResult(ExtractResult(result, third.Key), third.Key, third.Args),
                FormatResult(ExtractResult(result, fourth.Key), fourth.Key, fourth.Args),
                FormatResult(ExtractResult(result, fifth.Key), fifth.Key, fifth.Args),
                FormatResult(ExtractResult(result, sixth.Key), sixth.Key, sixth.Args)
            );
        }
        
        public async Task<(string First, string Second, string Third, string Fourth, string Fifth, string Sixth, string Seventh)>
            ResolveAsync(
                (TelegramTranslationKeys Key, object[] Args) first,
                (TelegramTranslationKeys Key, object[] Args) second,
                (TelegramTranslationKeys Key, object[] Args) third,
                (TelegramTranslationKeys Key, object[] Args) fourth,
                (TelegramTranslationKeys Key, object[] Args) fifth,
                (TelegramTranslationKeys Key, object[] Args) sixth,
                (TelegramTranslationKeys Key, object[] Args) seventh
            )
        {
            var update = userContextProvider.Update;
            var languageCode = ResolveLanguage(update);
            var result = await hookrRepository.GetTranslationsAsync(languageCode, false,
                first.Key,
                second.Key,
                third.Key,
                fourth.Key,
                fifth.Key,
                sixth.Key);
            return (
                FormatResult(ExtractResult(result, first.Key), first.Key, first.Args),
                FormatResult(ExtractResult(result, second.Key), second.Key, second.Args),
                FormatResult(ExtractResult(result, third.Key), third.Key, third.Args),
                FormatResult(ExtractResult(result, fourth.Key), fourth.Key, fourth.Args),
                FormatResult(ExtractResult(result, fifth.Key), fifth.Key, fifth.Args),
                FormatResult(ExtractResult(result, sixth.Key), sixth.Key, sixth.Args),
                FormatResult(ExtractResult(result, seventh.Key), seventh.Key, seventh.Args)
            );
        }

        private static string ExtractResult(IDictionary<TelegramTranslationKeys, string> dictionary, TelegramTranslationKeys key)
            => dictionary.TryGetValue(key, out var result)
                ? result
                : string.Empty;

        private static string FormatResult(string? result, TelegramTranslationKeys key, params object[] args)
            => string.IsNullOrEmpty(result)
                ? string.Format(NotFoundFormat, key, string.Join(',', args))
                : args.Any()
                    ? string.Format(result, args)
                    : result;

        private static LanguageCodes ResolveLanguage(ExtendedUpdate update)
            => Enum.TryParse<LanguageCodes>(update.Type == UpdateType.CallbackQuery
                    ? update.CallbackQuery.From.LanguageCode
                    : update.RealMessage.From.LanguageCode,
                true,
                out var parsedCode)
                ? parsedCode
                : DefaultLanguage;
    }
}