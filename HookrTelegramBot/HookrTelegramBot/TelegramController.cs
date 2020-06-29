using System.Threading.Tasks;
using HookrTelegramBot.Operations;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace HookrTelegramBot
{
    [Route("api/telegram")]
    public class TelegramController
    {
        private readonly IDispatcher dispatcher;

        public TelegramController(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }
        [HttpGet]
        public Task Getter() => dispatcher.DispatchAsync(new Update());
    }
}