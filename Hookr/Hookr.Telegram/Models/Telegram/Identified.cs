using System.Diagnostics.CodeAnalysis;

namespace Hookr.Telegram.Models.Telegram
{
    public class Identified<T>
    {
        [AllowNull] 
        public T Entity { get; set; } = default!;
        public int Index { get; set; }
    }
}