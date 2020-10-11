using System.Collections.Generic;
using System.Linq;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context;
using Hookr.Core.Repository.Context.Entities.Products;
using Hookr.Core.Repository.Context.Entities.Products.Photo;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Hookr.Telegram.Operations.Commands.Administration.Hookahs.Photos
{
    public class DeleteHookahPhotosCommand : DeletePhotosCommandBase<Hookah, HookahPhoto>, IDeleteHookahPhotosCommand
    {
        public DeleteHookahPhotosCommand(IExtendedTelegramBotClient telegramBotClient,
            ITranslationsResolver translationsResolver,
            IUserContextProvider userContextProvider,
            ITelegramHookrRepository hookrRepository)
            : base(telegramBotClient,
                translationsResolver,
                userContextProvider,
                hookrRepository)
        {
        }

        protected override DbSet<Hookah> EntityTableSelector(HookrContext context)
            => context.Hookahs;

        protected override DbSet<HookahPhoto> PhotoTableSelector(HookrContext context)
            => context.HookahPhotos;

        protected override IEnumerable<HookahPhoto>? PhotosSelector(Hookah product)
            => product.Photos;

        protected override IIncludableQueryable<Hookah, ICollection<HookahPhoto>?> IncludePhotosQuery(
            IQueryable<Hookah> query)
            => query
                .Include(x => x.Photos);
    }
}