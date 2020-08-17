namespace HookrTelegramBot.Repository.Context.Entities.Translations
{
    public class Translation
    {
        public int Id { get; set; }
        public LanguageCodes Language { get; set; }
        public TranslationKeys Key { get; set; }
        public string Value { get; set; }
    }
}