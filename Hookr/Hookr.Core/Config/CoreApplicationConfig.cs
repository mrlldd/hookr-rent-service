using Hookr.Core.Config.Cache;
using Hookr.Core.Config.Database;
using Hookr.Core.Config.Telegram;

namespace Hookr.Core.Config
{
    public class CoreApplicationConfig : ICoreApplicationConfig
    {
        public CoreApplicationConfig()
        {
            Database = new DatabaseConfig();
            Telegram = new TelegramConfig();
            Cache = new CacheConfig();
        }
        
        public IDatabaseConfig Database { get; }
        public ITelegramConfig Telegram { get; }
        public ICacheConfig Cache { get; }
    }
}