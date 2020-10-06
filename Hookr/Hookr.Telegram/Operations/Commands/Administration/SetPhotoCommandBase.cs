using System.Linq;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Repository.Context.Entities.Products;
using Hookr.Core.Repository.Context.Entities.Products.Photo;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Models.Telegram.Exceptions;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Operations.Commands.Administration
{
    public abstract class SetPhotoCommandBase<TProduct, TPhoto> : AdministrationCommandBase<TProduct>
        where TProduct : Product
        where TPhoto : ProductPhoto
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IHookrRepository hookrRepository;
        private readonly IMemoryCache memoryCache;

        protected SetPhotoCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            ITranslationsResolver translationsResolver,
            IUserContextProvider userContextProvider,
            IMemoryCache memoryCache)
            : base(telegramBotClient,
                translationsResolver)
        {
            this.userContextProvider = userContextProvider;
            this.hookrRepository = hookrRepository;
            this.memoryCache = memoryCache;
        }

        protected abstract string ProductCacheKeyFormat { get; }

        protected sealed override async Task ProcessAsync()
        {
            var user = userContextProvider.DatabaseUser;
            var message = userContextProvider.Update.RealMessage;
            if (user.State < TelegramUserStates.Service)
            {
                throw new InsufficientAccessRightsException("Seems like you have no access.");
            }

            if (!message.Photo.Any())
            {
                throw new InvalidArgumentsPassedInException("There must be a photo.");
            }

            if (!memoryCache.TryGetValue<int>(string.Format(ProductCacheKeyFormat, user.Id), out var productId))
            {
                throw new InvalidArgumentsPassedInException("There must be an integer.");
            }

            if (!await hookrRepository
                .ReadAsync(
                    (context, token) => EntityTableSelector(context)
                        .AnyAsync(x => x.Id == productId, token)
                )
            )
            {
                throw new InvalidArgumentsPassedInException($"Product with id {productId} does not exist.");
            }

            AddPhotoToTable(PhotoTableSelector(hookrRepository.Context), message.Photo.First().FileId, productId);
            await hookrRepository.Context.SaveChangesAsync();
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => await client.SendTextMessageAsync(
                await TranslationsResolver
                    .ResolveAsync(TelegramTranslationKeys.AddPhotoSuccess)
                );

        protected abstract DbSet<TPhoto> PhotoTableSelector(HookrContext context);

        protected abstract EntityEntry<TPhoto> AddPhotoToTable(DbSet<TPhoto> table, string fileId, int productId);
    }
}