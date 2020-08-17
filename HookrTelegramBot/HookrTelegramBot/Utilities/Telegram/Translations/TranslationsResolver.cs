using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace HookrTelegramBot.Utilities.Telegram.Translations
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

        public async Task<string> ResolveAsync(TranslationKeys translationKey, params object[] args)
        {
            var update = userContextProvider.Update;
            var languageCode = ResolveLanguage(update);
            var result = await hookrRepository.GetTranslationAsync(languageCode, translationKey);
            return FormatResult(result, translationKey, args);
        }

        public async Task<(string First, string Second)> ResolveAsync((TranslationKeys Key, object[] Args) first,
            (TranslationKeys Key, object[] Args) second)
        {
            var update = userContextProvider.Update;
            var languageCode = ResolveLanguage(update);
            var result = await hookrRepository.GetTranslationsAsync(languageCode, false, first.Key, second.Key);
            return (
                FormatResult(ExtractResult(result, first.Key), first.Key, first.Args),
                FormatResult(ExtractResult(result, second.Key), second.Key, second.Args)
            );
        }

        public Task<(string First, string Second)> ResolveAsync(TranslationKeys first, TranslationKeys second)
            => ResolveAsync(
                (first, Array.Empty<object>()),
                (second, Array.Empty<object>())
            );

        public async Task<(string First, string Second, string Third)> ResolveAsync(
            (TranslationKeys Key, object[] Args) first,
            (TranslationKeys Key, object[] Args) second,
            (TranslationKeys Key, object[] Args) third)
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
            (TranslationKeys Key, object[] Args) first,
            (TranslationKeys Key, object[] Args) second,
            (TranslationKeys Key, object[] Args) third,
            (TranslationKeys Key, object[] Args) fourth)
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
            (TranslationKeys Key, object[] Args) first,
            (TranslationKeys Key, object[] Args) second,
            (TranslationKeys Key, object[] Args) third,
            (TranslationKeys Key, object[] Args) fourth,
            (TranslationKeys Key, object[] Args) fifth)
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

        private static string ExtractResult(IDictionary<TranslationKeys, string> dictionary, TranslationKeys key)
            => dictionary.TryGetValue(key, out var result)
                ? result
                : string.Empty;

        private static string FormatResult(string result, TranslationKeys key, params object[] args)
            => string.IsNullOrEmpty(result)
                ? string.Format(NotFoundFormat, key, string.Join(',', args))
                : args.Any()
                    ? string.Format(result, args)
                    : result;

        private static LanguageCodes ResolveLanguage(ExtendedUpdate update)
            => Enum.TryParse<LanguageCodes>(update.Type == UpdateType.CallbackQuery
                ? update.CallbackQuery.From.LanguageCode
                : update.RealMessage.From.LanguageCode, true, out var parsedCode)
                ? parsedCode
                : DefaultLanguage;
    }
}