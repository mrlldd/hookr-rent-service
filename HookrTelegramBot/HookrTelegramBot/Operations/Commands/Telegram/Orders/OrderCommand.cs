using System;
using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Get;
using HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Get;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Caches;
using HookrTelegramBot.Utilities.Telegram.Caches.UserTemporaryStatus;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders
{
    public class OrderCommand : CommandWithResponse, IOrderCommand
    {
        private readonly IUserTemporaryStatusCache userTemporaryStatusCache;

        public OrderCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserTemporaryStatusCache userTemporaryStatusCache)
            : base(telegramBotClient)
        {
            this.userTemporaryStatusCache = userTemporaryStatusCache;
        }

        protected override Task ProcessAsync()
        {
            userTemporaryStatusCache.Set(TelegramBotClient.WithCurrentUser.User.Id, UserTemporaryStatus.InOrder);
            return Task.CompletedTask;
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
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
                }));
    }
}