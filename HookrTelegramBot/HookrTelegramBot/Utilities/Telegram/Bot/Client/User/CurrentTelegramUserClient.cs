using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Utilities.Telegram.Bot.Client.User
{
    public class CurrentTelegramUserClient : ICurrentTelegramUserClient
    {
        private readonly ITelegramBotClient botClient;
        private readonly Chat chat;

        public CurrentTelegramUserClient(ITelegramBotClient botClient, Chat chat)
        {
            this.botClient = botClient;
            this.chat = chat;
        }

        public Task<Message> SendTextMessageAsync(string text)
            => botClient.SendTextMessageAsync(chat, text);
    }
}