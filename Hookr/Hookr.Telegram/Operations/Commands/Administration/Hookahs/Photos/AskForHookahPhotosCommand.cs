using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Caches.UserTemporaryStatus;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.Extensions.Caching.Memory;

namespace Hookr.Telegram.Operations.Commands.Administration.Hookahs.Photos
{
    public class AskForHookahPhotosCommand : AskForPhotoCommandBase, IAskForHookahPhotosCommand 
    {
        public AskForHookahPhotosCommand(IExtendedTelegramBotClient telegramBotClient,
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

        protected override UserTemporaryStatus NextUserState => UserTemporaryStatus.AskedForHookahPhotos;
        protected override string ProductCacheKeyFormat => Constants.ProductCacheKeyFormat;
    }
}