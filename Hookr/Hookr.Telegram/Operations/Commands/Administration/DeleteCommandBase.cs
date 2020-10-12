using System;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Bot.Client.CurrentUser;
using Hookr.Telegram.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Hookr.Telegram.Operations.Commands.Administration
{
    public abstract class DeleteCommandBase<TEntity> : AdministrationCommandBase<TEntity>
        where TEntity : Entity
    {
        protected const string Space = " ";

        private readonly IUserContextProvider userContextProvider;
        private readonly ITelegramHookrRepository hookrRepository;

        protected DeleteCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            ITelegramHookrRepository hookrRepository,
            ITranslationsResolver translationsResolver) : base(telegramBotClient, translationsResolver)
        {
            this.userContextProvider = userContextProvider;
            this.hookrRepository = hookrRepository;
        }

        protected override async Task ProcessAsync()
        {
            if (userContextProvider.DatabaseUser.State < TelegramUserStates.Service)
            {
                throw new InvalidOperationException("No access rights.");
            }

            var update = userContextProvider.Update;
            var id = ExtractIndex(update.Type == UpdateType.CallbackQuery ? update.CallbackQuery.Data : update.Message.Text);
            var entities = await hookrRepository.ReadAsync((context, token)
                => EntityTableSelector(context).ToArrayAsync(token));
            EntityTableSelector(hookrRepository.Context).Remove(entities[id - 1]);
            await hookrRepository.SaveChangesAsync();
        }

        protected abstract int ExtractIndex(string command);
        
        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => await client
                .SendTextMessageAsync(await TranslationsResolver.ResolveAsync(TelegramTranslationKeys.DeleteCommandSuccess, typeof(TEntity).Name));
    }
}