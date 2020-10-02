namespace Hookr.Telegram.Utilities.Telegram.Caches.UserTemporaryStatus
{
    public interface IUserTemporaryStatusCache
    {
        void Set(int userId, UserTemporaryStatus status);
        UserTemporaryStatus Get(int userId);
    }
}