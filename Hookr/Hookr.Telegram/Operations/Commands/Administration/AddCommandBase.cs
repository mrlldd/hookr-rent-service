using System;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Telegram.Bot.Types;

namespace Hookr.Telegram.Operations.Commands.Administration
{
    public abstract class AddCommandBase<TEntity> : AdministrationCommandBase<TEntity> where TEntity : Entity
    {
        private readonly ITelegramHookrRepository hookrRepository;
        private readonly IUserContextProvider userContextProvider;

        protected const string Separator = "-";

        protected AddCommandBase(IExtendedTelegramBotClient telegramBotClient,
            ITelegramHookrRepository hookrRepository,
            IUserContextProvider userContextProvider,
            ITranslationsResolver translationsResolver)
            : base(telegramBotClient,
                translationsResolver)
        {
            this.hookrRepository = hookrRepository;
            this.userContextProvider = userContextProvider;
        }

        protected override async Task ProcessAsync()
        {
            if (userContextProvider.DatabaseUser.State < TelegramUserStates.Service)
            {
                throw new InvalidOperationException("No access rights to do that :(");
            }

            EntityTableSelector(hookrRepository.Context)
                .Add(ParseEntityModel(userContextProvider.Update.RealMessage.Text));
            await hookrRepository.SaveChangesAsync();
        }

        protected abstract TEntity ParseEntityModel(string command);

        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => await client.SendTextMessageAsync(
                await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.AddCommandResult, typeof(TEntity)));
    }
}