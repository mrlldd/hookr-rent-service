namespace Hookr.Core.Config.Database
{
    public interface IDatabaseConfig
    {
        string ConnectionString { get; }
        int Retries { get; }
        int Timeout { get; }
    }
}