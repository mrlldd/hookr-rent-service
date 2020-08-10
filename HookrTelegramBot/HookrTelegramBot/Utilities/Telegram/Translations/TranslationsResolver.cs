using System;
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

        public Task<string> GetAsync(TranslationKeys translationKey)
        {
            var update = userContextProvider.Update;
            var languageCode = Enum.TryParse<LanguageCodes>(update.Type == UpdateType.CallbackQuery
                ? update.CallbackQuery.From.LanguageCode
                : update.RealMessage.From.LanguageCode, out var parsedCode)
                ? parsedCode
                : DefaultLanguage;
            return hookrRepository.GetTranslationAsync(languageCode, translationKey);
        }
    }
}