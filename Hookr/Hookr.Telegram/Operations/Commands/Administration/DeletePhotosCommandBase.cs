using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hookr.Telegram.Models.Telegram.Exceptions;
using Hookr.Telegram.Operations.Base;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Repository.Context;
using Hookr.Telegram.Repository.Context.Entities.Base;
using Hookr.Telegram.Repository.Context.Entities.Products;
using Hookr.Telegram.Repository.Context.Entities.Products.Photo;
using Hookr.Telegram.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Operations.Commands.Administration
{
    public abstract class DeletePhotosCommandBase<TProduct, TPhoto> : CommandWithResponse
        where TProduct : Product
        where TPhoto : ProductPhoto
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IHookrRepository hookrRepository;
        private const string Space = " ";

        protected DeletePhotosCommandBase(IExtendedTelegramBotClient telegramBotClient,
            ITranslationsResolver translationsResolver,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository)
            : base(telegramBotClient, translationsResolver)
        {
            this.userContextProvider = userContextProvider;
            this.hookrRepository = hookrRepository;
        }

        protected sealed override async Task ProcessAsync()
        {
            var dbUser = userContextProvider.DatabaseUser;
            if (dbUser.State < TelegramUserStates.Service)
            {
                throw new InsufficientAccessRightsException("You have no access.");
            }

            var productId = ExtractProductId(userContextProvider.Update.Content);
            var product = await hookrRepository
                .ReadAsync((context, token) => IncludePhotosQuery(EntityTableSelector(context))
                    .FirstOrDefaultAsync(x => x.Id == productId, token)
                );
            if (product == null)
            {
                throw new InvalidArgumentsPassedInException("Product does not exist.");
            }

            PhotoTableSelector(hookrRepository.Context).RemoveRange(PhotosSelector(product));
            await hookrRepository.Context.SaveChangesAsync();
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => await client.SendTextMessageAsync(
                await TranslationsResolver
                    .ResolveAsync(TelegramTranslationKeys.DeletePhotosSuccess)
            );

        protected abstract DbSet<TProduct> EntityTableSelector(HookrContext context);

        protected abstract DbSet<TPhoto> PhotoTableSelector(HookrContext context);
        protected abstract IEnumerable<TPhoto>? PhotosSelector(TProduct product);

        protected abstract IIncludableQueryable<TProduct, ICollection<TPhoto>?> IncludePhotosQuery(
            IQueryable<TProduct> query);

        private static int ExtractProductId(string command)
        {
            var subs = command
                .Split(Space);
            if (subs.Length != 2)
            {
                throw new InvalidArgumentsPassedInException("Subs length must be 2.");
            }

            return int.TryParse(subs.Last(), out var result)
                ? result
                : throw new InvalidOperationException("Wrong product id have been passed in.");
        }
    }
}