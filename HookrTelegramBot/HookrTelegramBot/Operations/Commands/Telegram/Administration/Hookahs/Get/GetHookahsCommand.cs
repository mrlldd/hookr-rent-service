using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Caches;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Get
{
    public class GetHookahsCommand : CommandWithResponse<Hookah[]>, IGetHookahsCommand
    {
        private readonly IHookrRepository hookrRepository;
        private readonly IUserTemporaryStatusCache userTemporaryStatusCache;

        public GetHookahsCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserTemporaryStatusCache userTemporaryStatusCache) : base(telegramBotClient)
        {
            this.hookrRepository = hookrRepository;
            this.userTemporaryStatusCache = userTemporaryStatusCache;
        }

        protected override Task<Hookah[]> ProcessAsync()
            => hookrRepository
                .ReadAsync((context, token)
                    => context.Hookahs.ToArrayAsync(token))
                .ContinueWith(x =>
                {
                    if (x.IsCompletedSuccessfully)
                    {
                        userTemporaryStatusCache.Set(TelegramBotClient.WithCurrentUser.User.Id, UserTemporaryStatus.WaitingForHookah);
                    }

                    return x.Result;
                });

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, Hookah[] response)
            => client
                .SendTextMessageAsync(response
                    .Select((x, index) => $"/{index + 1} {x.Name} - {x.Price}")
                    .Aggregate((prev, next) => prev + "\n" + next));
    }
}