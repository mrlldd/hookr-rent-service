using HookrTelegramBot.Repository;
using HookrTelegramBot.Repository.Context.Entities;
using HookrTelegramBot.Utilities.Telegram.Bot;
using HookrTelegramBot.Utilities.Telegram.Bot.Client;
using HookrTelegramBot.Utilities.Telegram.Notifiers;
using HookrTelegramBot.Utilities.Telegram.Translations;

namespace HookrTelegramBot.Operations.Commands.Telegram.Orders.Control.Service.Review.Confirmed
{
    public abstract class ReviewConfirmedOrderCommandBase : ReviewOrderCommandBase
    {
        protected ReviewConfirmedOrderCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            IHookrRepository hookrRepository,
            ITranslationsResolver translationsResolver,
            ITelegramUsersNotifier telegramUsersNotifier)
            : base(telegramBotClient,
                userContextProvider, 
                hookrRepository,
                translationsResolver,
                telegramUsersNotifier)
        {
        }

        protected sealed override OrderStates AllowedState => OrderStates.Confirmed;
    }
}