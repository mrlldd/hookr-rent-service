using System.Diagnostics.CodeAnalysis;

namespace Hookr.Core.Repository.Context.Entities.Translations
{
    public class Translation<TKey>
    {
        public int Id { get; set; }
        public LanguageCodes Language { get; set; }
        [NotNull] public TKey Key { get; set; } = default!;
        [NotNull]
        public string? Value { get; set; }
    }
}