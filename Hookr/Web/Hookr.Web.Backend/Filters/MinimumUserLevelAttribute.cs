using System;
using System.Linq;
using Hookr.Core.Repository.Context.Entities.Base;
using Microsoft.AspNetCore.Authorization;

namespace Hookr.Web.Backend.Filters
{
    public class MinimumUserLevelAttribute : AuthorizeAttribute
    {
        public MinimumUserLevelAttribute(params TelegramUserStates[] minimum)
        {
            Roles = string.Join(',', Enum.GetValues(typeof(TelegramUserStates))
                .OfType<TelegramUserStates>()
                .Except(minimum)
                .Select(x => x.ToString("G"))
            );
        }
    }
}