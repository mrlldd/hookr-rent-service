using System;
using System.Linq;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Models.Telegram.Exceptions;
using Hookr.Telegram.Operations.Base;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Caches.UserTemporaryStatus;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Operations.Commands.Administration
{
    public abstract class AskForPhotoCommandBase : CommandWithResponse
    {
        private readonly IUserTemporaryStatusCache userTemporaryStatusCache;
        private readonly IUserContextProvider userContextProvider;
        private readonly IMemoryCache memoryCache;
        private const double ExpirationMinutes = 1;

        private const string Space = " ";

        protected AskForPhotoCommandBase(IExtendedTelegramBotClient telegramBotClient,
            ITranslationsResolver translationsResolver,
            IUserTemporaryStatusCache userTemporaryStatusCache,
            IUserContextProvider userContextProvider,
            IMemoryCache memoryCache)
            : base(telegramBotClient,
                translationsResolver)
        {
            this.userTemporaryStatusCache = userTemporaryStatusCache;
            this.userContextProvider = userContextProvider;
            this.memoryCache = memoryCache;
        }

        protected override async Task ProcessAsync()
        {
            var user = userContextProvider.DatabaseUser;
            if (user.State < TelegramUserStates.Service)
            {
                throw new InsufficientAccessRightsException("Seems like you have no access.");
            }

            var content = userContextProvider.Update.Content;

            var key = string.Format(ProductCacheKeyFormat, user.Id);
            memoryCache.Set(
                key,
                ExtractProductId(content),
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(ExpirationMinutes)
                }
            );
            await userTemporaryStatusCache.SetAsync(NextUserState);
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => await client.SendTextMessageAsync(
                await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.SendPhotosAsGroup)
            );

        protected abstract UserTemporaryStatus NextUserState { get; }

        protected abstract string ProductCacheKeyFormat { get; }

        private static int ExtractProductId(string command)
        {
            var subs = command
                .Split(Space);
            if (subs.Length != 2)
            {
                throw new InvalidArgumentsPassedInException("Seems like wrong arguments have been passed in.");
            }

            return int.TryParse(subs.Last(), out var result)
                ? result
                : throw new InvalidOperationException("There must be an integer.");
        }
    }
}