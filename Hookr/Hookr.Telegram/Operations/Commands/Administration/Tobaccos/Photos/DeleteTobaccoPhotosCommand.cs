using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Repository.Context;
using Hookr.Telegram.Repository.Context.Entities.Products;
using Hookr.Telegram.Repository.Context.Entities.Products.Photo;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Hookr.Telegram.Operations.Commands.Administration.Tobaccos.Photos
{
    public class DeleteTobaccoPhotosCommand : DeletePhotosCommandBase<Tobacco, TobaccoPhoto>, IDeleteTobaccoPhotosCommand
    {
        public DeleteTobaccoPhotosCommand(IExtendedTelegramBotClient telegramBotClient,
            ITranslationsResolver translationsResolver,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository) 
            : base(telegramBotClient,
                translationsResolver,
                userContextProvider,
                hookrRepository)
        {
        }

        protected override DbSet<Tobacco> EntityTableSelector(HookrContext context)
            => context.Tobaccos;

        protected override DbSet<TobaccoPhoto> PhotoTableSelector(HookrContext context)
            => context.TobaccoPhotos;
        
        protected override IEnumerable<TobaccoPhoto>? PhotosSelector(Tobacco product)
            => product.Photos;

        protected override IIncludableQueryable<Tobacco, ICollection<TobaccoPhoto>?> IncludePhotosQuery(
            IQueryable<Tobacco> query)
            => query
                .Include(x => x.Photos);
    }
}