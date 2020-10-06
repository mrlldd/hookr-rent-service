using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Repository.Context.Entities.Products;
using Hookr.Core.Repository.Context.Entities.Products.Ordered;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Operations.Commands.Orders.Control.Confirm;
using Hookr.Telegram.Operations.Commands.Orders.Control.Service.Review.Confirmed.Approve;
using Hookr.Telegram.Operations.Commands.Orders.Control.Service.Review.Confirmed.Reject;
using Hookr.Telegram.Operations.Commands.Orders.Control.Service.Review.Finish;
using Hookr.Telegram.Operations.Commands.Orders.Control.Service.Review.Process;
using Hookr.Telegram.Operations.Commands.Orders.Delete;
using Hookr.Telegram.Utilities.Extensions;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using EnumerableExtensions = Hookr.Core.Utilities.Extensions.EnumerableExtensions;

namespace Hookr.Telegram.Operations.Commands.Orders.Get
{
    public class GetOrderCommand : OrderCommandBase,
        IGetOrderCommand
    {
        public GetOrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository,
                translationsResolver)
        {
        }

        protected override IQueryable<Order> SideQuery(IQueryable<Order> query)
            => query
                .Include(x => x.OrderedHookahs)
                .ThenInclude(x => x.Product)
                .Include(x => x.OrderedTobaccos)
                .ThenInclude(x => x.Product);

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client,
            Order response)
        {
            var buttons = new List<IEnumerable<InlineKeyboardButton>>();
            var firstLayerButtons = new List<InlineKeyboardButton>();
            var builder = new StringBuilder();
            if (response.OrderedHookahs.Any() || response.OrderedTobaccos.Any())
            {
                builder.Append(StringifyResponse(response));
                if (response.State == OrderStates.Constructing)
                {
                    firstLayerButtons.Add(new InlineKeyboardButton
                    {
                        Text = await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.Confirm),
                        CallbackData = $"/{nameof(ConfirmOrderCommand).ExtractCommandName()} {response.Id}"
                    });
                }
            }
            else
            {
                builder.Append("seems like there is no any data in order");
            }

            builder.Append($"\n\nStatus: *{response.State}*");

            if (response.State == OrderStates.Constructing)
            {
                firstLayerButtons.Add(new InlineKeyboardButton
                {
                    Text = await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.Delete),
                    CallbackData = $"/{nameof(DeleteOrderCommand).ExtractCommandName()} {response.Id}"
                });
            }

            buttons.Add(firstLayerButtons);
            if (UserContextProvider.DatabaseUser.State > TelegramUserStates.Default)
            {
                buttons.Add(await GetServiceButtonsAsync(response));
            }

            return await client
                .SendTextMessageAsync(builder.ToString(),
                    ParseMode.MarkdownV2,
                    replyMarkup: new InlineKeyboardMarkup(buttons));
        }

        private async Task<IEnumerable<InlineKeyboardButton>> GetServiceButtonsAsync(Order order)
        {
            switch (order.State)
            {
                case OrderStates.Confirmed:
                {
                    var (approveTranslated, rejectTranslated) =
                        await TranslationsResolver
                            .ResolveAsync(TelegramTranslationKeys.Approve, TelegramTranslationKeys.Reject);
                    return new[]
                    {
                        new InlineKeyboardButton
                        {
                            Text = approveTranslated,
                            CallbackData = $"/{nameof(ApproveOrderCommand).ExtractCommandName()} {order.Id}"
                        },
                        new InlineKeyboardButton
                        {
                            Text = rejectTranslated,
                            CallbackData = $"/{nameof(RejectOrderCommand).ExtractCommandName()} {order.Id}"
                        },
                    };
                }
                case OrderStates.Approved:
                    return new[]
                    {
                        new InlineKeyboardButton
                        {
                            Text = await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.Process),
                            CallbackData = $"/{nameof(ProcessOrderCommand).ExtractCommandName()} {order.Id}"
                        },
                    };
                case OrderStates.Processing:
                    return new[]
                    {
                        new InlineKeyboardButton
                        {
                            Text = await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.Finish),
                            CallbackData = $"/{nameof(FinishOrderCommand).ExtractCommandName()} {order.Id}"
                        },
                    };
                case OrderStates.Constructing:
                case OrderStates.Rejected:
                case OrderStates.Finished:
                    return Array.Empty<InlineKeyboardButton>();
                case OrderStates.Unknown:
                default:
                    throw new ArgumentOutOfRangeException(nameof(order.State), order.State, null);
            }
        }

        private static string StringifyResponse(Order order)
            => (order.OrderedTobaccos.Any()
                   ? "Tobaccos:" +
                     AggregateProducts(order.OrderedTobaccos)
                   : string.Empty) +
               (order.OrderedHookahs.Any()
                   ? "\n\nHookahs:" +
                     AggregateProducts(order.OrderedHookahs)
                   : string.Empty);

        private static string AggregateProducts<TProduct>([AllowNull] IEnumerable<Ordered<TProduct>> products)
            where TProduct : Product
            => new StringBuilder()
                .SideEffect(builder => EnumerableExtensions.ForEach(products, x => builder.Append($"\n{x.Product?.Name} \\- {x.Count}"))
                )
                .ToString();
    }
}