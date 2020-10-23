using System.Text;

namespace Hookr.Core.Utilities.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string Utf8String(this byte[] utf8)
            => Encoding.UTF8.GetString(utf8);
    }
}