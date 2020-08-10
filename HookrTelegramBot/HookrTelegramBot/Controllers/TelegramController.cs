using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.ActionFilters;
using HookrTelegramBot.Operations;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Controllers
{
    [Route("api/telegram")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        private readonly IDispatcher dispatcher;
        private readonly IHookrRepository hookrRepository;

        public TelegramController(IDispatcher dispatcher,
            IHookrRepository hookrRepository)
        {
            this.dispatcher = dispatcher;
            this.hookrRepository = hookrRepository;
        }

        [ServiceFilter(typeof(CurrentTelegramUpdateGrabber))]
        [HttpPost("update")]
        public Task Update([FromBody] Update _) => dispatcher.DispatchAsync();

        [HttpPost("translations")]
        public Task UpdateTranslations(
            [FromBody] Dictionary<LanguageCodes, Dictionary<TranslationKeys, string>> newTranslations)
        {
            if (newTranslations == null 
                || !newTranslations.Any()
                || newTranslations.Values
                    .Any(x => x.Count == 0))
            {
                HttpContext.Response.StatusCode = 403;
                return Task.CompletedTask;
            }

            var table = hookrRepository.Context.Translations;
            newTranslations.ForEach(pair =>
            {
                var (language, inner) = pair;
                inner.ForEach(x => table.Add(new Translation
                {
                    Key = x.Key,
                    Value = x.Value,
                    Language = language
                }));
            });
            return hookrRepository.Context.SaveChangesAsync();
        }
    }
}