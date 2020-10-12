namespace Hookr.Core.Utilities.Loaders
{
    public interface ILoaderProvider
    {
        CachingLoader<TArgs, TResult> Get<TArgs, TResult>()
            where TResult : class;
    }
}