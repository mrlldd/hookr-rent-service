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

namespace Hookr.Telegram.Operations.Commands.Administration.Tobaccos.Photos
{
    public class SetTobaccoPhotosCommand : SetPhotoCommandBase<Tobacco, TobaccoPhoto>, ISetTobaccoPhotosCommand
    {
        public SetTobaccoPhotosCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository, 
            ITranslationsResolver translationsResolver,
            IUserContextProvider userContextProvider,
            IMemoryCache memoryCache)
            : base(telegramBotClient,
                hookrRepository,
                translationsResolver,
                userContextProvider,
                memoryCache)
        {
        }

        protected override string ProductCacheKeyFormat => Constants.ProductCacheKeyFormat;

        protected override DbSet<Tobacco> EntityTableSelector(HookrContext context)
            => context.Tobaccos;

        protected override DbSet<TobaccoPhoto> PhotoTableSelector(HookrContext context)
            => context.TobaccoPhotos;

        protected override EntityEntry<TobaccoPhoto> AddPhotoToTable(DbSet<TobaccoPhoto> table,
            string fileId,
            int productId)
            => table.Add(new TobaccoPhoto
            {
                TobaccoId = productId,
                TelegramFileId = fileId
            });
    }
}