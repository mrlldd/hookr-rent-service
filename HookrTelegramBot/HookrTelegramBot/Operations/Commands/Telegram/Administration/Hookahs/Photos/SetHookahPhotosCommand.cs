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

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Photos
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

        protected override string ProductCacheKeyFormat => "shp{0}";

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