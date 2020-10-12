using Hookr.Core.Repository.Context.Entities;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Hookr.Telegram.Utilities.Telegram.Bot.Client;
using Hookr.Telegram.Utilities.Telegram.Notifiers;
using Hookr.Telegram.Utilities.Telegram.Translations;

namespace Hookr.Telegram.Operations.Commands.Orders.Control.Service.Review.Confirmed
{
    public abstract class ReviewConfirmedOrderCommandBase : ReviewOrderCommandBase
    {
        protected ReviewConfirmedOrderCommandBase(IExtendedTelegramBotClient telegramBotClient,
            IUserContextProvider userContextProvider,
            ITelegramHookrRepository hookrRepository,
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