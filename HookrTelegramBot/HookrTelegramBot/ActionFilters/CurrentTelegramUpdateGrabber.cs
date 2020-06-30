using System.Linq;
using HookrTelegramBot.Utilities.Telegram.Bot;
using Microsoft.AspNetCore.Mvc.Filters;
using Telegram.Bot.Types;

namespace HookrTelegramBot.ActionFilters
{
    public class CurrentTelegramUpdateGrabber : ActionFilterAttribute
    {
        private readonly ICurrentUpdateProvider currentUpdateProvider;

        public CurrentTelegramUpdateGrabber(ICurrentUpdateProvider currentUpdateProvider)
        {
            this.currentUpdateProvider = currentUpdateProvider;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
            => currentUpdateProvider.Set(
                context.ActionArguments.Values.FirstOrDefault(x => x is Update) as Update);
    }
}