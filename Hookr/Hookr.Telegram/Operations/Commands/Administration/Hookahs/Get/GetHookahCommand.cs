using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Repository.Context.Entities.Products;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Models.Telegram;
using Hookr.Telegram.Operations.Commands.Administration.Hookahs.Delete;
using Hookr.Telegram.Operations.Commands.Administration.Hookahs.Photos;
using Hookr.Telegram.Operations.Commands.Orders.Control.AddProduct;
using Hookr.Telegram.Utilities.Extensions;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Caches.CurrentOrder;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Hookr.Telegram.Operations.Commands.Administration.Hookahs.Get
{
    public class GetHookahCommand : GetSingleCommandBase<Hookah>, IGetHookahCommand
    {
        private readonly ICurrentOrderCache currentOrderCache;

        public GetHookahCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
            ICurrentOrderCache currentOrderCache,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository,
                translationsResolver)
        {
            this.currentOrderCache = currentOrderCache;
        }

        protected override int ExtractIndex(string command)
            => int
                .TryParse(command
                    .Trim()
                    .Substring(1), out var result)
                ? result
                : throw new InvalidOperationException("Wrong arguments.");

        protected override DbSet<Hookah> EntityTableSelector(HookrContext context)
            => context.Hookahs;

        protected override IQueryable<Hookah> SideQuery(IQueryable<Hookah> query)
            => query
                .Include(x => x.Photos);

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client,
            Identified<Hookah> response)
        {
            var (content, keyboard) = await (TranslationsResolver.ResolveAsync(TelegramTranslationKeys.GetHookahResult,
                    response.Entity.Name,
                    response.Entity.Price),
                PrepareKeyboardAsync(response)).CombineAsync();
            if (response.Entity.Photos.Any())
            {
                await client
                    .SendMediaGroupAsync(response.Entity.Photos
                        .Select(x =>
                            new InputMediaPhoto(new InputMedia(x.TelegramFileId))
                        )
                    );
            }

            return await client
                .SendTextMessageAsync(
                    content,
                    replyMarkup: keyboard);
        }

        private async Task<InlineKeyboardMarkup> PrepareKeyboardAsync(Identified<Hookah> hookah)
        {
            const byte defaultCount = 1;
            var buttons = new List<IEnumerable<InlineKeyboardButton>>();
            var dbUser = UserContextProvider.DatabaseUser;
            var orderId = await currentOrderCache.GetAsync();
            if (orderId.HasValue)
            {
                buttons.Add(new[]
                {
                    new InlineKeyboardButton
                    {
                        Text = await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.OrderSomething),
                        CallbackData =
                            $"/{nameof(AddToOrderCommand).ExtractCommandName()} {orderId} {nameof(Hookah)} {hookah.Index} {defaultCount}"
                    }
                });
            }

            if (dbUser.State > TelegramUserStates.Default)
            {
                var (key, command) = hookah.Entity.Photos.Any()
                    ? (TelegramTranslationKeys.DeletePhotos, nameof(DeleteHookahPhotosCommand))
                    : (TelegramTranslationKeys.SetPhotos, nameof(AskForHookahPhotosCommand));

                var (deleteTranslation, secondTranslation) = await TranslationsResolver
                    .ResolveAsync(
                        TelegramTranslationKeys.Delete,
                        key
                    );
                buttons.AddRange(new[]
                {
                    new[]
                    {
                        new InlineKeyboardButton
                        {
                            Text = deleteTranslation,
                            CallbackData = $"/{nameof(DeleteHookahCommand).ExtractCommandName()} {hookah.Index}"
                        }
                    },
                    new[]
                    {
                        new InlineKeyboardButton
                        {
                            Text = secondTranslation,
                            CallbackData =
                                $"/{command.ExtractCommandName()} {hookah.Entity.Id}"
                        },
                    }
                });
            }

            return new InlineKeyboardMarkup(buttons);
        }
    }
}