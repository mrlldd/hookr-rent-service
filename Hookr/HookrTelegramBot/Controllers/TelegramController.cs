using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.ActionFilters;
using HookrTelegramBot.Operations;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task UpdateTranslations(
            [FromBody] Dictionary<LanguageCodes, Dictionary<TranslationKeys, string>> newTranslations)
        {
            // todo password key validation
            if (newTranslations == null
                || !newTranslations.Any()
                || newTranslations.Values
                    .Any(x => !x.Any()))
            {
                HttpContext.Response.StatusCode = 400;
                return;
            }

            var languages = newTranslations.Keys;
            var translations = await hookrRepository
                .ReadAsync((context, token) => context.Translations
                    .Where(x => languages.Contains(x.Language))
                    .GroupBy(x => x.Language)
                    .ToArrayAsync(token)
                );
            var table = hookrRepository.Context.Translations;
            translations
                .ForEach(grouping =>
                {
                    newTranslations[grouping.Key]
                        .ForEach(x =>
                        {
                            var (key, value) = x;
                            var existing = grouping
                                .FirstOrDefault(y => y.Key == key);
                            if (existing == null)
                            {
                                table.Add(new Translation
                                {
                                    Key = key,
                                    Value = value,
                                    Language = grouping.Key
                                });
                            }
                            else
                            {
                                existing.Value = value;
                                table.Update(existing);
                            }
                        });
                });
            await hookrRepository.Context.SaveChangesAsync();
        }
    }
}