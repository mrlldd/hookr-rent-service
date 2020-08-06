using System;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Get;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Get;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Caches.CurrentOrder;
using HookrTelegramBot.Utilities.Telegram.Caches.UserTemporaryStatus;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders
{
    public class OrderCommand : CommandWithResponse<InlineKeyboardButton[]>, IOrderCommand
    {
        private readonly IUserTemporaryStatusCache userTemporaryStatusCache;
        private readonly IHookrRepository hookrRepository;
        private readonly ICurrentOrderCache currentOrderCache;
        private readonly IUserContextProvider userContextProvider;

        public OrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserTemporaryStatusCache userTemporaryStatusCache,
            IHookrRepository hookrRepository,
            ICurrentOrderCache currentOrderCache,
            IUserContextProvider userContextProvider)
            : base(telegramBotClient)
        {
            this.userTemporaryStatusCache = userTemporaryStatusCache;
            this.hookrRepository = hookrRepository;
            this.currentOrderCache = currentOrderCache;
            this.userContextProvider = userContextProvider;
        }

        protected override async Task<InlineKeyboardButton[]> ProcessAsync()
        {
            userTemporaryStatusCache.Set(TelegramBotClient.WithCurrentUser.User.Id,
                UserTemporaryStatus.InOrder);
            var cachedOrderId = currentOrderCache.Get(userContextProvider.DatabaseUser.Id);
            if (cachedOrderId.HasValue)
                return new[]
                {
                    new InlineKeyboardButton
                    {
                        Text = "View current order",
                        CallbackData = $"/getorder {cachedOrderId}"
                        //todo
                    }
                };
            var order = new Order();
            await hookrRepository.Context.Orders.AddAsync(order);
            await hookrRepository.Context.SaveChangesAsync();
            currentOrderCache.Set(userContextProvider.DatabaseUser.Id, order.Id);
            return Array.Empty<InlineKeyboardButton>();

        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client,
            InlineKeyboardButton[] response)
            => client
                .SendTextMessageAsync("Choose product", replyMarkup: new InlineKeyboardMarkup(new[]
                    {
                        new InlineKeyboardButton
                        {
                            Text = "Hookahs",
                            CallbackData = $"/{nameof(GetHookahsCommand).ExtractCommandName()}"
                        },
                        new InlineKeyboardButton
                        {
                            Text = "Tobaccos",
                            CallbackData = $"/{nameof(GetTobaccosCommand).ExtractCommandName()}"
                        },
                    }
                    .Concat(response)
                    .ToArray()));
    }
}