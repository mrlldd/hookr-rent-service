using Newtonsoft.Json;

namespace HookrTelegramBot.Utilities.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
            => JsonConvert.SerializeObject(obj);
    }
}