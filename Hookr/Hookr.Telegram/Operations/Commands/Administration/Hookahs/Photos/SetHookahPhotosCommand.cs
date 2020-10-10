using Hookr.Core.Repository;
using Hookr.Core.Repository.Context;
using Hookr.Core.Repository.Context.Entities.Products;
using Hookr.Core.Repository.Context.Entities.Products.Photo;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Memory;

namespace Hookr.Telegram.Operations.Commands.Administration.Hookahs.Photos
{
    public class SetHookahPhotosCommand : SetPhotoCommandBase<Hookah, HookahPhoto>, ISetHookahPhotosCommand
    {
        public SetHookahPhotosCommand(IExtendedTelegramBotClient telegramBotClient,
            ITranslationsResolver translationsResolver,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
            IMemoryCache memoryCache)
            : base(telegramBotClient,
                hookrRepository,
                translationsResolver,
                userContextProvider,
                memoryCache)
        {
        }

        protected override string ProductCacheKeyFormat => Constants.ProductCacheKeyFormat;

        protected override DbSet<Hookah> EntityTableSelector(HookrContext context)
            => context.Hookahs;

        protected override DbSet<HookahPhoto> PhotoTableSelector(HookrContext context)
            => context.HookahPhotos;

        protected override EntityEntry<HookahPhoto> AddPhotoToTable(DbSet<HookahPhoto> table,
            string fileId,
            int productId)
            => table.Add(new HookahPhoto
            {
                HookahId = productId,
                TelegramFileId = fileId
            });
    }
}