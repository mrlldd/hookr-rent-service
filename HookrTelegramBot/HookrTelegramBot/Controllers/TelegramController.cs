using System.Threading.Tasks;
using HookrTelegramBot.ActionFilters;
using HookrTelegramBot.Operations;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Controllers
{
    [Route("api/telegram")]
    public class TelegramController
    {
        private readonly IDispatcher dispatcher;

        public TelegramController(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }
        
        [ServiceFilter(typeof(CurrentTelegramUpdateGrabber))]
        [HttpPost("update")]
        public Task Update([FromBody]Update _) => dispatcher.DispatchAsync();
    }
}