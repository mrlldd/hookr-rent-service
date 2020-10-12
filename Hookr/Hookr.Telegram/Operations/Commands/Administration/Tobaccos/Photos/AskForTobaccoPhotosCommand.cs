using Hookr.Core.Utilities.Caches;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Caches.UserTemporaryStatus;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.Extensions.Caching.Memory;

namespace Hookr.Telegram.Operations.Commands.Administration.Tobaccos.Photos
{
    public class AskForTobaccoPhotosCommand : AskForPhotoCommandBase, IAskForTobaccoPhotosCommand
    {
        public AskForTobaccoPhotosCommand(IExtendedTelegramBotClient telegramBotClient,
            ITranslationsResolver translationsResolver,
            ICacheProvider cacheProvider,
            IUserContextProvider userContextProvider,
            IMemoryCache memoryCache)
            : base(telegramBotClient,
                translationsResolver,
                cacheProvider,
                userContextProvider,
                memoryCache)
        {
        }

        protected override UserTemporaryStatus NextUserState => UserTemporaryStatus.AskedForTobaccoPhotos;
        protected override string ProductCacheKeyFormat => Constants.ProductCacheKeyFormat;

    }
}