using System;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Hookahs.Add
{
    public class AddHookahCommand : AddCommandBase<Hookah>, IAddHookahCommand
    {
        public AddHookahCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserContextProvider userContextProvider,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                hookrRepository,
                userContextProvider,
                translationsResolver)
        {
        }

        protected override DbSet<Hookah> EntityTableSelector(HookrContext context)
            => context.Hookahs;

        protected override Hookah ParseEntityModel(string command)
        {
            var subs = command
                .Substring(command.IndexOf(" ", StringComparison.Ordinal))
                .Split(Separator)
                .Select(x => x.Trim())
                .ToArray();
            if (subs.Length != 2 || !int.TryParse(subs[1], out var price))
            {
                throw new InvalidOperationException("Wrongs arguments passed in.");
            }

            return new Hookah
            {
                Name = subs[0],
                Price = price
            };
        }

    }
}