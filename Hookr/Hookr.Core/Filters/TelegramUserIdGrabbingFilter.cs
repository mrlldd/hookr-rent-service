using System.Threading.Tasks;
using Hookr.Core.Utilities.Providers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hookr.Core.Filters
{
    public abstract class TelegramUserIdGrabbingFilter : IAsyncActionFilter
    {
        private readonly ITelegramUserIdProvider telegramUserIdProvider;

        protected TelegramUserIdGrabbingFilter(ITelegramUserIdProvider telegramUserIdProvider)
        {
            this.telegramUserIdProvider = telegramUserIdProvider;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            telegramUserIdProvider.Set(Grab(context));
            return next();
        }

        protected abstract int Grab(ActionExecutingContext context);
    }
}