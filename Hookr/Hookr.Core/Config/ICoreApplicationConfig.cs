using Hookr.Core.Config.Cache;
using Hookr.Core.Config.Database;
using Hookr.Core.Config.Telegram;

namespace Hookr.Core.Config
{
    public interface ICoreApplicationConfig
    {
        IDatabaseConfig Database { get; }
        ITelegramConfig Telegram { get; }
        ICacheConfig Cache { get; }
    }
}