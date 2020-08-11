using System;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration
{
    public abstract class AddCommandBase<TEntity> : AdministrationCommandBase<TEntity> where TEntity : Entity
    {
        private readonly IHookrRepository hookrRepository;
        private readonly IUserContextProvider userContextProvider;
        private readonly ITranslationsResolver translationsResolver;

        protected const string Separator = "-";

        protected AddCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserContextProvider userContextProvider,
            ITranslationsResolver translationsResolver) : base(telegramBotClient)
        {
            this.hookrRepository = hookrRepository;
            this.userContextProvider = userContextProvider;
            this.translationsResolver = translationsResolver;
        }

        protected override async Task ProcessAsync()
        {
            if (userContextProvider.DatabaseUser.State < TelegramUserStates.Service)
            {
                throw new InvalidOperationException("No access rights to do that :(");
            }

            EntityTableSelector(hookrRepository.Context)
                .Add(ParseEntityModel(userContextProvider.Update.RealMessage.Text));
            await hookrRepository.Context.SaveChangesAsync();
        }

        protected abstract TEntity ParseEntityModel(string command);

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => await client.SendTextMessageAsync(
                await translationsResolver.ResolveAsync(TranslationKeys.AddCommandResult, typeof(TEntity)));
    }
}