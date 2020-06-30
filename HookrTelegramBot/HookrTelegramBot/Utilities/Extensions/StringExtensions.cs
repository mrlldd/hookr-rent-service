using System.Linq;

namespace HookrTelegramBot.Utilities.Extensions
{
    public static class StringExtensions
    {
        private const string Space = " ";
        public static string ExtractCommandName(this string s)
            => s
                   .Replace("Command", Space)
                   .Split(Space)
                   .FirstOrDefault()?
                   .ToLower() ?? s;

    }
}