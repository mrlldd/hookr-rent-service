using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Delete;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Delete;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Photos;
using HookrTelegramBot.Operations.Commands.Telegram.Orders;
using HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.AddProduct;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Repository.Context.Entities.Products;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Caches.CurrentOrder;
using HookrTelegramBot.Utilities.Telegram.Caches.UserTemporaryStatus;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Get
{
    public class GetTobaccoCommand : GetSingleCommandBase<Tobacco>, IGetTobaccoCommand
    {
        private readonly ICurrentOrderCache currentOrderCache;

        public GetTobaccoCommand(IExtendedTelegramBotClient telegramBotClient,
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

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client,
            Identified<Tobacco> response)
        {
            var (content, keyboard) = await (
                TranslationsResolver.ResolveAsync(TranslationKeys.GetTobaccoResult, response.Entity.Name,
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
            var orderId = currentOrderCache.Get(dbUser.Id);
            if (orderId.HasValue)
            {
                var orderSomeGramsFormat = await TranslationsResolver
                    .ResolveAsync(TranslationKeys.OrderSomeGrams);
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
                var (deleteTranslation, setPhotosTranslation) = await TranslationsResolver
                    .ResolveAsync(
                        TranslationKeys.Delete,
                        TranslationKeys.SetPhotos
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
                            Text = setPhotosTranslation,
                            CallbackData =
                                $"/{nameof(AskForTobaccoPhotosCommand).ExtractCommandName()} {tobacco.Entity.Id}"
                        }
                    }
                });
            }

            return new InlineKeyboardMarkup(buttons);
        }
    }
}