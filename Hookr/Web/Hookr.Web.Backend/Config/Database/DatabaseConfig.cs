namespace Hookr.Web.Backend.Config.Database
{
    public class DatabaseConfig : IDatabaseConfig
    {
        public string ConnectionString { get; set; }
        public int Retries { get; set; }
        public int Timeout { get; set; }
    }
}