using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Caches.UserTemporaryStatus;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Microsoft.Extensions.Caching.Memory;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Photos
{
    public class AskForTobaccoPhotosCommand : AskForPhotoCommandBase, IAskForTobaccoPhotosCommand
    {
        public AskForTobaccoPhotosCommand(IExtendedTelegramBotClient telegramBotClient,
            ITranslationsResolver translationsResolver,
            IUserTemporaryStatusCache userTemporaryStatusCache,
            IUserContextProvider userContextProvider,
            IMemoryCache memoryCache)
            : base(telegramBotClient,
                translationsResolver,
                userTemporaryStatusCache,
                userContextProvider,
                memoryCache)
        {
        }

        protected override UserTemporaryStatus NextUserState => UserTemporaryStatus.AskedForTobaccoPhotos;
        protected override string ProductCacheKeyFormat => Constants.ProductCacheKeyFormat;

    }
}