using System;
using System.Threading.Tasks;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Delete
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