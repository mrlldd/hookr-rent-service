using System;
using System.Linq;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Operations.Base;
using Hookr.Telegram.Operations.Commands.Administration.Hookahs.Get;
using Hookr.Telegram.Operations.Commands.Administration.Tobaccos.Get;
using Hookr.Telegram.Operations.Commands.Orders.Get;
using Hookr.Telegram.Utilities.Extensions;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Caches.CurrentOrder;
using Hookr.Telegram.Utilities.Telegram.Caches.UserTemporaryStatus;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Hookr.Telegram.Operations.Commands.Orders
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
            IUserContextProvider userContextProvider,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                translationsResolver)
        {
            this.userTemporaryStatusCache = userTemporaryStatusCache;
            this.hookrRepository = hookrRepository;
            this.currentOrderCache = currentOrderCache;
            this.userContextProvider = userContextProvider;
        }

        protected override async Task<InlineKeyboardButton[]> ProcessAsync()
        {
            await userTemporaryStatusCache.SetAsync(UserTemporaryStatus.InOrder);
            var cachedOrderId = await currentOrderCache.GetAsync();
            if (cachedOrderId.HasValue)
                return new[]
                {
                    new InlineKeyboardButton
                    {
                        Text = await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.ViewCurrentOrder),
                        CallbackData = $"/{nameof(GetOrderCommand).ExtractCommandName()} {cachedOrderId}"
                        //todo
                    }
                };
            var order = new Order();
            hookrRepository.Context.Orders.Add(order);
            await hookrRepository.Context.SaveChangesAsync();
            await currentOrderCache.SetAsync(order.Id);
            return Array.Empty<InlineKeyboardButton>();
        }

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client,
            InlineKeyboardButton[] response)
        {
            var (product, hookahs, tobaccos) = await TranslationsResolver.ResolveAsync(
                (TelegramTranslationKeys.ChooseProduct, Array.Empty<object>()),
                (TelegramTranslationKeys.Hookahs, Array.Empty<object>()),
                (TelegramTranslationKeys.Tobaccos, Array.Empty<object>())
            );
            return await client
                .SendTextMessageAsync(product,
                    replyMarkup: new InlineKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                new InlineKeyboardButton
                                {
                                    Text = hookahs,
                                    CallbackData = $"/{nameof(GetHookahsCommand).ExtractCommandName()}"
                                },
                                new InlineKeyboardButton
                                {
                                    Text = tobaccos,
                                    CallbackData = $"/{nameof(GetTobaccosCommand).ExtractCommandName()}"
                                }
                            },
                            response
                        }
                        .ToArray()));
        }
    }
}