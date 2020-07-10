using System;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Delete
{
    public class DeleteHookahCommand : CommandWithResponse, IDeleteHookahCommand
    {
        private const string Space = " ";
        private readonly IUserContextProvider userContextProvider;
        private readonly IHookrRepository hookrRepository;

        public DeleteHookahCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository) : base(telegramBotClient)
        {
            this.userContextProvider = userContextProvider;
            this.hookrRepository = hookrRepository;
        }

        protected override async Task ProcessAsync()
        {
            var dbUser = userContextProvider.DatabaseUser;
            if (dbUser.State < TelegramUserStates.Service)
            {
                throw new InvalidOperationException("No access rights.");
            }

            var id = ExtractArguments(userContextProvider.Update.CallbackQuery.Data);
            var hookahs = await hookrRepository.ReadAsync((context, token)
                => context.Hookahs.ToArrayAsync(token));
            hookrRepository.Context.Hookahs.Remove(hookahs[id - 1]);
            await hookrRepository.Context.SaveChangesAsync();
        }

        private static int ExtractArguments(string commandText)
            => int
                .Parse(commandText
                    .Split(Space)[1]
                    .Trim());

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => client
                .SendTextMessageAsync("Hookah was successfully removed.");
    }
}