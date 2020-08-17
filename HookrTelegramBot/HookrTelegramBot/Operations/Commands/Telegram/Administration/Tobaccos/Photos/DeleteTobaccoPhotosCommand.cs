using System.Collections.Generic;
using System.Linq;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities.Products;
using HookrTelegramBot.Repository.Context.Entities.Products.Photo;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Photos
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

        protected override IEnumerable<TobaccoPhoto> PhotosSelector(Tobacco product)
            => product.Photos;

        protected override IIncludableQueryable<Tobacco, ICollection<TobaccoPhoto>> IncludePhotosQuery(
            IQueryable<Tobacco> query)
            => query
                .Include(x => x.Photos);
    }
}