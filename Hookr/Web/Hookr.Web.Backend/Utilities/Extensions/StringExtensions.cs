using System.Text;

namespace Hookr.Web.Backend.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static byte[] Utf8Bytes(this string str)
            => Encoding.UTF8.GetBytes(str);
    }
}