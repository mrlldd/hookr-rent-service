using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Base
{
    public interface ICommand
    {
        public Task ExecuteAsync();
    }
}