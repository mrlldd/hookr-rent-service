using System.Linq;
using Hookr.Core.Repository.Context.Entities.Base;
using Microsoft.AspNetCore.Authorization;

namespace Hookr.Web.Backend.Filters.Auth
{
    public class OnlyFor : AuthorizeAttribute
    {
        public OnlyFor(params TelegramUserStates[] roles)
        {
            Roles = string
                .Join(',', roles
                    .Select(x => x.ToString("G"))
                );
        }
    }
}