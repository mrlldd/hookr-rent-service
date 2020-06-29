using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Utilities.Telegram.Selectors;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Start
{
    public class StartCommand : CommandWithResponse, IStartCommand
    {
        public StartCommand(IChatSelector chatSelector) : base(chatSelector)
        {
        }

        protected override Task ProcessAsync(ExtendedUpdate update) => Task.CompletedTask;

        protected override Task<Message> SendResponseAsync(Chat chat)
        {
            throw new System.NotImplementedException();
        }
    }
}