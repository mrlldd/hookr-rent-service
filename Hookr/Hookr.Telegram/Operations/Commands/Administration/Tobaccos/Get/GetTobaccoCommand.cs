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
using Hookr.Telegram.Operations.Commands.Administration.Tobaccos.Delete;
using Hookr.Telegram.Operations.Commands.Administration.Tobaccos.Photos;
using Hookr.Telegram.Operations.Commands.Orders.Control.AddProduct;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Extensions;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Caches.CurrentOrder;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Hookr.Telegram.Operations.Commands.Administration.Tobaccos.Get
{
    public class GetTobaccoCommand : GetSingleCommandBase<Tobacco>, IGetTobaccoCommand
    {
        private readonly ICurrentOrderCache currentOrderCache;

        public GetTobaccoCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            ITelegramHookrRepository hookrRepository,
            ICurrentOrderCache currentOrderCache,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository,
                translationsResolver)
        {
            this.currentOrderCache = currentOrderCache;
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client,
            Identified<Tobacco> response)
        {
            var (content, keyboard) = await (
                TranslationsResolver.ResolveAsync(TelegramTranslationKeys.GetTobaccoResult, response.Entity.Name,
                    response.Entity.Price),
                PrepareKeyboardAsync(response)
            ).CombineAsync();

            if (response.Entity.Photos.Any())
            {
                await client.SendMediaGroupAsync(
                    response.Entity.Photos
                        .Select(x => new InputMediaPhoto(new InputMedia(x.TelegramFileId)))
                );
            }

            return await client
                .SendTextMessageAsync(
                    content,
                    replyMarkup: keyboard);
        }

        protected override DbSet<Tobacco> EntityTableSelector(HookrContext context)
            => context.Tobaccos;

        protected override IQueryable<Tobacco> SideQuery(IQueryable<Tobacco> query)
            => query
                .Include(x => x.Photos);

        protected override int ExtractIndex(string command)
            => int
                .TryParse(command
                    .Trim()
                    .Substring(1), out var result)
                ? result
                : throw new InvalidOperationException("Wrong arguments.");

        private async Task<InlineKeyboardMarkup> PrepareKeyboardAsync(Identified<Tobacco> tobacco)
        {
            const byte defaultCount = 5;
            var buttons = new List<IEnumerable<InlineKeyboardButton>>();
            var dbUser = UserContextProvider.DatabaseUser;
            var orderId = await currentOrderCache.GetAsync();
            if (orderId.HasValue)
            {
                var orderSomeGramsFormat = await TranslationsResolver
                    .ResolveAsync(TelegramTranslationKeys.OrderSomeGrams);
                var innerButtons = new List<InlineKeyboardButton>();
                for (var i = defaultCount; i < defaultCount * 4 + 1; i *= 2)
                {
                    innerButtons.Add(new InlineKeyboardButton
                    {
                        Text = string.Format(orderSomeGramsFormat, i),
                        CallbackData =
                            $"/{nameof(AddToOrderCommand).ExtractCommandName()} {orderId} {nameof(Tobacco)} {tobacco.Index} {i}"
                    });
                }

                buttons.Add(innerButtons);
            }

            if (dbUser.State > TelegramUserStates.Default)
            {
                var (key, command) = tobacco.Entity.Photos.Any()
                    ? (TelegramTranslationKeys.DeletePhotos, nameof(DeleteTobaccoPhotosCommand))
                    : (TelegramTranslationKeys.SetPhotos, nameof(AskForTobaccoPhotosCommand));
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
                            CallbackData = $"/{nameof(DeleteTobaccoCommand).ExtractCommandName()} {tobacco.Index}"
                        }
                    },
                    new[]
                    {
                        new InlineKeyboardButton
                        {
                            Text = secondTranslation,
                            CallbackData =
                                $"/{command.ExtractCommandName()} {tobacco.Entity.Id}"
                        }
                    }
                });
            }

            return new InlineKeyboardMarkup(buttons);
        }
    }
}