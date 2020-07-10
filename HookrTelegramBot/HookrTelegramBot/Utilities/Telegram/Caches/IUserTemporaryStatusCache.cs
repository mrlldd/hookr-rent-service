using HookrTelegramBot.Models.Telegram;

namespace HookrTelegramBot.Utilities.Telegram.Caches
{
    public interface IUserTemporaryStatusCache
    {
        void Set(int userId, UserTemporaryStatus status);
        UserTemporaryStatus Get(int userId);
    }
}