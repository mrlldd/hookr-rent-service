using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Confirm;
using HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Service.Review.Confirmed.Approve;
using HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Service.Review.Confirmed.Reject;
using HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Service.Review.Finish;
using HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Service.Review.Process;
using HookrTelegramBot.Operations.Commands.Telegram.Orders.Delete;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Repository.Context.Entities.Products;
using HookrTelegramBot.Repository.Context.Entities.Products.Ordered;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders.Get
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
                        Text = await TranslationsResolver.ResolveAsync(TranslationKeys.Confirm),
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
                    Text = await TranslationsResolver.ResolveAsync(TranslationKeys.Delete),
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
                            .ResolveAsync(TranslationKeys.Approve, TranslationKeys.Reject);
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
                            Text = await TranslationsResolver.ResolveAsync(TranslationKeys.Process),
                            CallbackData = $"/{nameof(ProcessOrderCommand).ExtractCommandName()} {order.Id}"
                        },
                    };
                case OrderStates.Processing:
                    return new[]
                    {
                        new InlineKeyboardButton
                        {
                            Text = await TranslationsResolver.ResolveAsync(TranslationKeys.Finish),
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

        private static string AggregateProducts<TProduct>(IEnumerable<Ordered<TProduct>> products)
            where TProduct : Product
            => new StringBuilder()
                .SideEffect(builder => products
                    .ForEach(x => builder.Append($"\n{x.Product.Name} \\- {x.Count}"))
                )
                .ToString();
    }
}