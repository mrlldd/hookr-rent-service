using System;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context;
using Hookr.Core.Repository.Context.Entities.Products;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;

namespace Hookr.Telegram.Operations.Commands.Administration.Tobaccos.Delete
{
    public class DeleteTobaccoCommand : DeleteCommandBase<Tobacco>, IDeleteTobaccoCommand
    {
        public DeleteTobaccoCommand(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                userContextProvider,
                hookrRepository,
                translationsResolver)
        {
        }

        protected override DbSet<Tobacco> EntityTableSelector(HookrContext context)
            => context.Tobaccos;


        protected override int ExtractIndex(string command)
            => int
                .TryParse(command
                    .Split(Space)[1]
                    .Trim(), out var result)
                ? result
                : throw new InvalidOperationException("Wrong arguments.");
    }
}