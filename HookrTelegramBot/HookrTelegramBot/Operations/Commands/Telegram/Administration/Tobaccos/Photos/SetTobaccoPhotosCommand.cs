using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities.Products;
using HookrTelegramBot.Repository.Context.Entities.Products.Photo;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Memory;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Photos
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

        protected override string ProductCacheKeyFormat => Constants.TobaccoProductCacheKey;

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