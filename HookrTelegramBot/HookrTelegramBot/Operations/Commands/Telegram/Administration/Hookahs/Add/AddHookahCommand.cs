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

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Add
{
    public class AddHookahCommand : AddCommandBase<Hookah>, IAddHookahCommand
    {
        private const string Space = " ";

        public AddHookahCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserContextProvider userContextProvider)
            : base(telegramBotClient,
                hookrRepository,
                userContextProvider)
        {
        }

        protected override DbSet<Hookah> EntityTableSelector(HookrContext context)
            => context.Hookahs;

        protected override Hookah ParseEntityModel(string command)
        {
            var subs = command.Split(Space);
            if (subs.Length != 3 || !int.TryParse(subs[2], out var price))
            {
                throw new InvalidOperationException("Wrongs arguments passed in.");
            }

            return new Hookah
            {
                Name = subs[1],
                Price = price
            };
        }

        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => client.SendTextMessageAsync("Successfully added new hookah");
    }
}