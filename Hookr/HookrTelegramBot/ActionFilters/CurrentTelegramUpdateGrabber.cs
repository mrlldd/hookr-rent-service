using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Notifiers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HookrTelegramBot.ActionFilters
{
    public class CurrentTelegramUpdateGrabber : ActionFilterAttribute
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly ITelegramUsersNotifier telegramUsersNotifier;
        private readonly IHookrRepository hookrRepository;

        public CurrentTelegramUpdateGrabber(IUserContextProvider userContextProvider,
            ITelegramUsersNotifier telegramUsersNotifier,
            IHookrRepository hookrRepository)
        {
            this.userContextProvider = userContextProvider;
            this.telegramUsersNotifier = telegramUsersNotifier;
            this.hookrRepository = hookrRepository;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var extendedUpdate = userContextProvider.Set(
                context.ActionArguments.Values.FirstOrDefault(x => x is Update) as Update);
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