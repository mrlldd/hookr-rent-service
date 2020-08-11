using System;
using System.Linq;
using System.Threading.Tasks;
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

        public TranslationsResolver(IHookrRepository hookrRepository,
            IUserContextProvider userContextProvider)
        {
            this.hookrRepository = hookrRepository;
            this.userContextProvider = userContextProvider;
        }

        public async Task<string> ResolveAsync(TranslationKeys translationKey, params object[] args)
        {
            var update = userContextProvider.Update;
            var languageCode = Enum.TryParse<LanguageCodes>(update.Type == UpdateType.CallbackQuery
                ? update.CallbackQuery.From.LanguageCode
                : update.RealMessage.From.LanguageCode, true, out var parsedCode)
                ? parsedCode
                : DefaultLanguage;
            var result = await hookrRepository.GetTranslationAsync(languageCode, translationKey);
            return string.IsNullOrEmpty(result)
                ? $"[{translationKey}] [{string.Join(',', args)}]"
                : args.Any()
                    ? string.Format(result, args)
                    : result;
        }
    }
}