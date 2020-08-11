using System;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Repository.Context.Entities.Translations;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using HookrTelegramBot.Utilities.Telegram.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration
{
    public abstract class DeleteCommandBase<TEntity> : AdministrationCommandBase<TEntity>
        where TEntity : Entity
    {
        protected const string Space = " ";

        private readonly IUserContextProvider userContextProvider;
        private readonly IHookrRepository hookrRepository;
        protected readonly ITranslationsResolver TranslationsResolver;

        protected DeleteCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
            ITranslationsResolver translationsResolver) : base(telegramBotClient)
        {
            this.userContextProvider = userContextProvider;
            this.hookrRepository = hookrRepository;
            this.TranslationsResolver = translationsResolver;
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
            await hookrRepository.Context.SaveChangesAsync();
        }

        protected abstract int ExtractIndex(string command);
        
        protected override async Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => await client
                .SendTextMessageAsync(await TranslationsResolver.ResolveAsync(TranslationKeys.DeleteCommandSuccess, typeof(TEntity).Name));
    }
}