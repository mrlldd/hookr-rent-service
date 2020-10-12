namespace Hookr.Core.Utilities.Loaders
{
    public interface ILoaderProvider
    {
        ICachingLoader<TArgs, TResult> Get<TArgs, TResult>()
            where TResult : class;
    }
}