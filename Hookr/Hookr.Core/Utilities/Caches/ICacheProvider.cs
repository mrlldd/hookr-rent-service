namespace Hookr.Core.Utilities.Caches
{
    public interface ICacheProvider
    {
        ICache<T> UserLevel<T>();

        ICache<T> ApplicationLevel<T>();
    }
}