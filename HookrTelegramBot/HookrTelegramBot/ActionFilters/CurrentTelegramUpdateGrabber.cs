using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.ActionFilters
{
    public class CurrentTelegramUpdateGrabber : ActionFilterAttribute
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IExtendedTelegramBotClient telegramBotClient;
        private readonly IHookrRepository hookrRepository;

        public CurrentTelegramUpdateGrabber(IUserContextProvider userContextProvider,
            IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository)
        {
            this.userContextProvider = userContextProvider;
            this.telegramBotClient = telegramBotClient;
            this.hookrRepository = hookrRepository;
        }
 
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            
            var extendedUpdate = userContextProvider.Set(
                context.ActionArguments.Values.FirstOrDefault(x => x is Update) as Update);;
            var result = await next();
            if (result.Exception != null)
            {
                var devs = await hookrRepository.ReadAsync((hookrContext, token) =>
                    hookrContext.TelegramUsers
                        .Where(x => x.State == TelegramUserStates.Dev)
                        .ToArrayAsync(token));
                await Task.WhenAll(devs
                    .Select(x => telegramBotClient.SendTextMessageAsync(new ChatId(x.Username), "exception handled"))
                    .Append(telegramBotClient.SendTextMessageAsync(extendedUpdate.Chat,"There is an error :("))
                );
                result.ExceptionHandled = true;
                result.HttpContext.Response.StatusCode = 200;
            }
        }
    }
}