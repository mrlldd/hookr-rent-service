using System;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration
{
    public abstract class AddCommandBase<TEntity> : AdministrationCommandBase<TEntity> where TEntity : Entity
    {
        private readonly IHookrRepository hookrRepository;
        private readonly IUserContextProvider userContextProvider;
        
        protected const string Separator = "-";

        protected AddCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserContextProvider userContextProvider) : base(telegramBotClient)
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

            await EntityTableSelector(hookrRepository.Context)
                .AddAsync(ParseEntityModel(userContextProvider.Update.RealMessage.Text));
            await hookrRepository.Context.SaveChangesAsync();
        }

        protected abstract TEntity ParseEntityModel(string command);
        
        protected override Task<Message> SendResponseAsync(ICurrentTelegramUserClient client)
            => client.SendTextMessageAsync($"Successfully added new {typeof(TEntity).Name}.");
    }
}