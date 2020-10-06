using Hookr.Web.Backend.Config.Database;
using Hookr.Web.Backend.Config.Telegram;

namespace Hookr.Web.Backend.Config
{
    public class ApplicationConfig : IApplicationConfig
    {
        public ApplicationConfig()
        {
            Database = new DatabaseConfig();
            Telegram = new TelegramConfig();
        }
        public IDatabaseConfig Database { get; }
        public ITelegramConfig Telegram { get; }
    }
}