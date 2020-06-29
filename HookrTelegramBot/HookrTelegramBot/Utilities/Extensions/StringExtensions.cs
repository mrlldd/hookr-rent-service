using System.Linq;

namespace HookrTelegramBot.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static string ExtractCommandName(this string s)
            => s
                   .Replace("Command", " ")
                   .Split(" ")
                   .FirstOrDefault()?
                   .ToLower() ?? s;

    }
}