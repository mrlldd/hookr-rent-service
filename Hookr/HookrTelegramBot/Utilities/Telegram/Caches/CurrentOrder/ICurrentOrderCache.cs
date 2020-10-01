namespace HookrTelegramBot.Utilities.Telegram.Caches.CurrentOrder
{
    public interface ICurrentOrderCache
    {
        void Set(int userId, int orderId);
        int? Get(int userId);
    }
}