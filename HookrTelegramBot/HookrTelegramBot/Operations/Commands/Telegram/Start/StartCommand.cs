using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HookrTelegramBot.Operations.Commands.Telegram.Start
{
    public class StartCommand : CommandWithResponse, IStartCommand
    {
        public StartCommand(IExtendedTelegramBotClient telegramBotClient)
            : base(telegramBotClient)
        {
        }

        protected override Task ProcessAsync() => Task.CompletedTask;

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => client
                .SendTextMessageAsync("Hey!", replyMarkup: new ReplyKeyboardMarkup
                {
                    OneTimeKeyboard = true,
                    ResizeKeyboard = true,
                    Keyboard = new[]
                    {
                        new[]
                        {
                            new KeyboardButton
                            {
                                Text = "hi",
                            },
                            new KeyboardButton
                            {
                                Text = "Order some"
                            },
                        }
                    }
                });
    }
}