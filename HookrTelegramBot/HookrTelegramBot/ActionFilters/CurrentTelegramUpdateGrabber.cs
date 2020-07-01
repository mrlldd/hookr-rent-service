using System.Linq;
using HookrTelegramBot.Utilities.Telegram.Bot;
using Microsoft.AspNetCore.Mvc.Filters;
using Telegram.Bot.Types;

namespace HookrTelegramBot.ActionFilters
{
    public class CurrentTelegramUpdateGrabber : ActionFilterAttribute
    {
        private readonly IUserContextProvider userContextProvider;

        public CurrentTelegramUpdateGrabber(IUserContextProvider userContextProvider)
        {
            this.userContextProvider = userContextProvider;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
            => userContextProvider.Set(
                context.ActionArguments.Values.FirstOrDefault(x => x is Update) as Update);
    }
}