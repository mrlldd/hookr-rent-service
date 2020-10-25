namespace Hookr.Core.Config.Cache
{
    public interface ICacheConfig
    {
        string ConnectionString { get; }
        int LinearRetries { get; }
        int KeepAliveSeconds { get; }
    }
}