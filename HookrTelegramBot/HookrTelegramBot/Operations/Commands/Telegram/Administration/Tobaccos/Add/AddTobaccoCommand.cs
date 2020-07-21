﻿using System;
using System.Linq;
using System.Threading.Tasks;
using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Bot.Client.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace HookrTelegramBot.Operations.Commands.Telegram.Administration.Tobaccos.Add
{
    public class AddTobaccoCommand : AddCommandBase<Tobacco>, IAddTobaccoCommand
    {
        public AddTobaccoCommand(IExtendedTelegramBotClient telegramBotClient,
            IHookrRepository hookrRepository,
            IUserContextProvider userContextProvider)
            : base(telegramBotClient,
                hookrRepository,
                userContextProvider)
        {
        }

        protected override DbSet<Tobacco> EntityTableSelector(HookrContext context)
            => context.Tobaccos;


        protected override Tobacco ParseEntityModel(string command)
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

            return new Tobacco
            {
                Name = subs[0],
                Price = price
            };
        }
    }
}