using System;
using System.Linq;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context;
using Hookr.Core.Repository.Context.Entities.Products;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;

namespace Hookr.Telegram.Operations.Commands.Administration.Hookahs.Add
{
    public class AddHookahCommand : AddCommandBase<Hookah>, IAddHookahCommand
    {
        public AddHookahCommand(IExtendedTelegramBotClient telegramBotClient,
            ITelegramHookrRepository hookrRepository,
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