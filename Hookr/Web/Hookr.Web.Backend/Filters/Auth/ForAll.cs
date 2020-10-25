using Hookr.Core.Repository.Context.Entities.Base;

namespace Hookr.Web.Backend.Filters.Auth
{
    public class ForAll : ForAllExcept
    {
        public ForAll() : base(TelegramUserStates.Unknown)
        {
        }
    }
}