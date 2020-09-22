using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
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
using HookrTelegramBot.Utilities.Telegram.Caches.CurrentOrder;
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
            string text;
            if (response.OrderedHookahs.Any() || response.OrderedTobaccos.Any())
            {
                text = StringifyResponse(response);
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
                text = "seems like there is no any data in order";
            }

            text += $"\n\nStatus: *{response.State}*";

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
                buttons.Add(GetServiceButtons(response));
            }

            return await client
                .SendTextMessageAsync(text,
                    ParseMode.MarkdownV2,
                    replyMarkup: new InlineKeyboardMarkup(buttons));
        }

        private IEnumerable<InlineKeyboardButton> GetServiceButtons(Order order)
        {
            return order.State switch
            {
                //todo translations
                OrderStates.Confirmed => new[]
                {
                    new InlineKeyboardButton
                    {
                        Text = "Approve",
                        CallbackData = $"/{nameof(ApproveOrderCommand).ExtractCommandName()} {order.Id}"
                    },
                    new InlineKeyboardButton
                    {
                        Text = "Reject",
                        CallbackData = $"/{nameof(RejectOrderCommand).ExtractCommandName()} {order.Id}"
                    },
                },
                OrderStates.Approved => new[]
                {
                    new InlineKeyboardButton
                    {
                        Text = "Process",
                        CallbackData = $"/{nameof(ProcessOrderCommand).ExtractCommandName()} {order.Id}"
                    },
                },
                OrderStates.Processing => new[]
                {
                    new InlineKeyboardButton
                    {
                        Text = "Finish",
                        CallbackData = $"/{nameof(FinishOrderCommand).ExtractCommandName()} {order.Id}"
                    },
                },
                OrderStates.Constructing => Array.Empty<InlineKeyboardButton>(),
                OrderStates.Rejected => Array.Empty<InlineKeyboardButton>(),
                OrderStates.Finished => Array.Empty<InlineKeyboardButton>(),
                OrderStates.Unknown => throw new ArgumentOutOfRangeException(nameof(order.State), order.State, null),
                _ => throw new ArgumentOutOfRangeException(nameof(order.State), order.State, null)
            };
        }

        private static string StringifyResponse(Order order) =>
            (order.OrderedTobaccos.Any()
                ? "Tobaccos:" +
                  AggregateProducts(order.OrderedTobaccos)
                : string.Empty) +
            (order.OrderedHookahs.Any()
                ? "\n\nHookahs:" +
                  AggregateProducts(order.OrderedHookahs)
                : string.Empty);

        private static string AggregateProducts<TProduct>(IEnumerable<Ordered<TProduct>> products)
            where TProduct : Product
            => products
                .Aggregate(string.Empty,
                    (prev, next) => prev + $"\n{next.Product.Name} \\- {next.Count}");
    }
}