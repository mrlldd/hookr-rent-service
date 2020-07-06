using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Admin.Register
{
    public class RegisterAdminCommand : CommandWithResponse<bool>, IRegisterAdminCommand
    {
        public RegisterAdminCommand(IExtendedTelegramBotClient telegramBotClient) : base(telegramBotClient)
        {
        }

        protected override Task<bool> ProcessAsync()
        {
            throw new System.NotImplementedException();
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client, bool response)
        {
            throw new System.NotImplementedException();
        }
    }
}