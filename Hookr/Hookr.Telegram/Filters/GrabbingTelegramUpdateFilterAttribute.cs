using System.Linq;
using System.Threading.Tasks;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Repository.Context.Entities.Base;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Notifiers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Hookr.Telegram.Filters
{
    public class GrabbingCurrentTelegramUpdateFilterAttribute : ActionFilterAttribute
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly ITelegramUsersNotifier telegramUsersNotifier;
        private readonly IHookrRepository hookrRepository;

        public GrabbingCurrentTelegramUpdateFilterAttribute(IUserContextProvider userContextProvider,
            ITelegramUsersNotifier telegramUsersNotifier,
            IHookrRepository hookrRepository)
        {
            this.userContextProvider = userContextProvider;
            this.telegramUsersNotifier = telegramUsersNotifier;
            this.hookrRepository = hookrRepository;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!(context.ActionArguments.Values.FirstOrDefault(x => x is Update) is Update update))
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            var extendedUpdate = userContextProvider.Set(update);
            var id = extendedUpdate.Type == UpdateType.CallbackQuery
                ? extendedUpdate.CallbackQuery.From.Id
                : extendedUpdate.RealMessage.From.Id;
            var dbUser = await hookrRepository
                .ReadAsync((hookrContext, token)
                    => hookrContext.TelegramUsers.FirstOrDefaultAsync(x => x.Id == id,
                        token));
            userContextProvider.SetDatabaseUser(dbUser);
            var result = await next();
            if (result.Exception != null)
            {
                await telegramUsersNotifier
                    .SendAsync((client, user) => client
                            .SendTextMessageAsync(user.Id, result.Exception.ToString()),
                        TelegramUserStates.Dev);
                result.ExceptionHandled = true;
                result.HttpContext.Response.StatusCode = 200;
            }
        }
    }
}