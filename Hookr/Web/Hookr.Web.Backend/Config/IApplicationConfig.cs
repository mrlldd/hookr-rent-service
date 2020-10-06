using Hookr.Web.Backend.Config.Database;
using Hookr.Web.Backend.Config.Telegram;

namespace Hookr.Web.Backend.Config
{
    public interface IApplicationConfig
    {
        IDatabaseConfig Database { get; }
        ITelegramConfig Telegram { get; }
        
    }
}