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

        public static string ExtractCommand(this string s)
            => s
                .Split(Space)
                .FirstOrDefault()?
                .ToLower()
                .Substring(1);

        public static bool IsNumber(this string s)
            => int.TryParse(s, out _);
    }
}