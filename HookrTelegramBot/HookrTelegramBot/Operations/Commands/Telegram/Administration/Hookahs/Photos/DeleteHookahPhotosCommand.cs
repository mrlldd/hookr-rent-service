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

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Photos
{
    public class DeleteHookahPhotosCommand : DeletePhotosCommandBase<Hookah, HookahPhoto>, IDeleteHookahPhotosCommand
    {
        public DeleteHookahPhotosCommand(IExtendedTelegramBotClient telegramBotClient,
            ITranslationsResolver translationsResolver,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository)
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

        protected override IEnumerable<HookahPhoto> PhotosSelector(Hookah product)
            => product.Photos;

        protected override IIncludableQueryable<Hookah, ICollection<HookahPhoto>> IncludePhotosQuery(
            IQueryable<Hookah> query)
            => query
                .Include(x => x.Photos);
    }
}