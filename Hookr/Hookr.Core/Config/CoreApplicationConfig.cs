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
        }
        public IDatabaseConfig Database { get; }
        public ITelegramConfig Telegram { get; }
    }
}