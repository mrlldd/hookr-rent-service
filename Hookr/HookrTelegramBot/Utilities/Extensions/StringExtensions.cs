using System.Linq;
using System.Text.RegularExpressions;

namespace HookrTelegramBot.Utilities.Extensions
{
    public static class StringExtensions
    {
        private const string Space = " ";
        private const string PreservedCharacters = @"[][.=>\-]";
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
        
        public static string FilterPreservedCharacters(this string target)
            => Regex.Replace(target,PreservedCharacters, "\\$&", RegexOptions.Compiled);
    }
}