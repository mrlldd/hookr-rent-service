using System;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Delete
{
    public class DeleteHookahCommand : DeleteCommandBase<Hookah>, IDeleteHookahCommand
    {
        private const string Space = " ";

        public DeleteHookahCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository)
        {
        }

        protected override DbSet<Hookah> EntityTableSelector(HookrContext context)
            => context.Hookahs;

        protected override int ExtractIndex(string command)
            => int
                .TryParse(command
                    .Split(Space)[1]
                    .Trim(), out var result)
        ? result
        : throw new InvalidOperationException("Wrong arguments.");

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => client
                .SendTextMessageAsync("Hookah was successfully removed.");
    }
}