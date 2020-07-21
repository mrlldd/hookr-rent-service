﻿using System;
using System.Threading.Tasks;
using HookrTelegramBot.Operations.Base;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Enums;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration
{
    public abstract class DeleteCommandBase<TEntity> : AdministrationCommandBase<TEntity>
        where TEntity : Entity
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IHookrRepository hookrRepository;

        protected DeleteCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository) : base(telegramBotClient)
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
            await hookrRepository.Context.SaveChangesAsync();
        }

        protected abstract int ExtractIndex(string command);
    }
}