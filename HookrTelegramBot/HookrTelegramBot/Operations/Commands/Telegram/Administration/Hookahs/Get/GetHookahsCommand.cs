using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Products;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Caches;
using HookrTelegramBot.Utilities.Telegram.Caches.UserTemporaryStatus;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Get
{
    public class GetHookahsCommand : GetCommandBase<Hookah>, IGetHookahsCommand
    {

        public GetHookahsCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserTemporaryStatusCache userTemporaryStatusCache,
            IUserContextProvider userContextProvider,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                hookrRepository,
                userTemporaryStatusCache,
                userContextProvider,
                translationsResolver)
        {
        }

        protected override DbSet<Hookah> EntityTableSelector(HookrContext context)
            => context.Hookahs;

        protected override UserTemporaryStatus NextUserState => UserTemporaryStatus.ChoosingHookah;
        
        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Hookah[] response)
            => await client
                .SendTextMessageAsync(response.Any()
                    ? response
                        .AggregateListString("/{0} {1} - {2}",
                            x => x.Name,
                            x => x.Price)
                    : await TranslationsResolver.ResolveAsync(TranslationKeys.NoHookahs));

    }
}