using System.Threading.Tasks;
using HookrTelegramBot.Models.Telegram;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Base
{
    public abstract class Command<TUpdate> where TUpdate : ExtendedUpdate
    {
        public abstract string Name { get; }
        public abstract Task ExecuteAsync(TUpdate update);
    }
}