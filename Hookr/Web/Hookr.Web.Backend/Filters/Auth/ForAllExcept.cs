using System;
using System.Linq;
using Hookr.Core.Repository.Context.Entities.Base;

namespace Hookr.Web.Backend.Filters.Auth
{
    public class ForAllExcept : OnlyFor
    {
        public ForAllExcept(params TelegramUserStates[] except)
            : base(Enum
                .GetValues(typeof(TelegramUserStates))
                .OfType<TelegramUserStates>()
                .Except(except
                    .Concat(new[]
                    {
                        TelegramUserStates.Unknown
                    })
                )
                .ToArray()
            )
        {
        }
    }
}