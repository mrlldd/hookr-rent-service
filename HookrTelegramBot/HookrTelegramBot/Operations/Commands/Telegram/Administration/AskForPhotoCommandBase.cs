using System;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram.Exceptions;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Caches.UserTemporaryStatus;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration
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

        protected override Task ProcessAsync()
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
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(ExpirationMinutes),
                    PostEvictionCallbacks =
                    {
                        new PostEvictionCallbackRegistration
                        {
                            EvictionCallback =
                                (o, value, reason, state)
                                    => userTemporaryStatusCache.Set(user.Id, UserTemporaryStatus.Default)
                        }
                    }
                }
            );
            userTemporaryStatusCache.Set(user.Id, NextUserState);
            return Task.CompletedTask;
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => await client.SendTextMessageAsync(
                await TranslationsResolver.ResolveAsync(TranslationKeys.SendPhotosAsGroup)
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