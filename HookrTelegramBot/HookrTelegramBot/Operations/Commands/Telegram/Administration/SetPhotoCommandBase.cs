using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram.Exceptions;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Repository.Context.Entities.Products;
using HookrTelegramBot.Repository.Context.Entities.Products.Photo;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration
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

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => client.SendTextMessageAsync("Photo added.");

        protected abstract DbSet<TPhoto> PhotoTableSelector(HookrContext context);

        protected abstract EntityEntry<TPhoto> AddPhotoToTable(DbSet<TPhoto> table, string fileId, int productId);

        
    }
}