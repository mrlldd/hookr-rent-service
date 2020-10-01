namespace HookrTelegramBot.Models.Telegram
{
    public class Identified<T>
    {
        public T Entity { get; set; }
        public int Index { get; set; }
    }
}