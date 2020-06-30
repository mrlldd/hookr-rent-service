using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Selectors;
using Serilog;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Start
{
    public class StartCommand : CommandWithResponse, IStartCommand
    {
        public StartCommand(IExtendedTelegramBotClient telegramBotClient,
            ICurrentUpdateProvider currentUpdateProvider)
            : base(telegramBotClient, currentUpdateProvider)
        {
        }

        protected override Task ProcessAsync() => Task.CompletedTask;

        protected override Task<Message> SendResponseAsync(IExtendedTelegramBotClient bot)
            => bot.SendTextMessageToCurrentUserAsync("Hey!");
    }
}