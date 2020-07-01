using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.User;
using Telegram.Bot.Types;

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
            => client.SendTextMessageAsync("Hey!");
    }
}