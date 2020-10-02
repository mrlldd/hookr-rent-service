using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Telegram.Utilities.Telegram.Bot.Provider;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;
using File = Telegram.Bot.Types.File;

#pragma warning disable 618

namespace Hookr.Telegram.Utilities.Telegram.Bot.Client
{
    public class DecoratedTelegramBotClientBase
    {
        private readonly ITelegramBotProvider provider;
        protected ITelegramBotClient Bot => provider.Instance;

        protected DecoratedTelegramBotClientBase(ITelegramBotProvider provider, bool omitEventProxies = false)
        {
            this.provider = provider;
            if (!omitEventProxies)
            {
                SetupEventProxies();
            }
        }

        private void SetupEventProxies()
        {
            Bot.MakingApiRequest += (sender, args) => MakingApiRequest?.Invoke(sender, args);
            Bot.ApiResponseReceived += (sender, args) => ApiResponseReceived?.Invoke(sender, args);
            Bot.OnUpdate += (sender, args) => OnUpdate?.Invoke(sender, args);
            Bot.OnMessage += (sender, args) => OnMessage?.Invoke(sender, args);
            Bot.OnMessageEdited += (sender, args) => OnMessageEdited?.Invoke(sender, args);
            Bot.OnInlineQuery += (sender, args) => OnInlineQuery?.Invoke(sender, args);
            Bot.OnInlineResultChosen += (sender, args) => OnInlineResultChosen?.Invoke(sender, args);
            Bot.OnCallbackQuery += (sender, args) => OnCallbackQuery?.Invoke(sender, args);
            Bot.OnReceiveError += (sender, args) => OnReceiveError?.Invoke(sender, args);
            Bot.OnReceiveGeneralError += (sender, args) => OnReceiveGeneralError?.Invoke(sender, args);
        }

        public virtual Task<TResponse> MakeRequestAsync<TResponse>(IRequest<TResponse> request,
            CancellationToken cancellationToken = default)
            => Bot
                .MakeRequestAsync(request, cancellationToken);

        public virtual Task<bool> TestApiAsync(CancellationToken cancellationToken = default)
            => Bot
                .TestApiAsync(cancellationToken);

        public virtual void StartReceiving(UpdateType[]? allowedUpdates = null,
            CancellationToken cancellationToken = default)
            => Bot
                .StartReceiving(allowedUpdates, cancellationToken);

        public virtual void StopReceiving()
            => Bot
                .StopReceiving();

        public virtual Task<Update[]> GetUpdatesAsync(int offset = 0,
            int limit = 0,
            int timeout = 0,
            IEnumerable<UpdateType>? allowedUpdates = null,
            CancellationToken cancellationToken = default)
            => Bot
                .GetUpdatesAsync(offset, limit, timeout, allowedUpdates, cancellationToken);

        public virtual Task SetWebhookAsync(string url,
            InputFileStream? certificate = null,
            int maxConnections = 0,
            IEnumerable<UpdateType>? allowedUpdates = null,
            CancellationToken cancellationToken = default)
            => Bot
                .SetWebhookAsync(url,
                    certificate,
                    maxConnections,
                    allowedUpdates,
                    cancellationToken);

        public virtual Task DeleteWebhookAsync(CancellationToken cancellationToken = default)
            => Bot
                .DeleteWebhookAsync(cancellationToken);

        public virtual Task<WebhookInfo> GetWebhookInfoAsync(CancellationToken cancellationToken = default)
            => Bot
                .GetWebhookInfoAsync(cancellationToken);

        public virtual Task<User> GetMeAsync(CancellationToken cancellationToken = default)
            => Bot
                .GetMeAsync(cancellationToken);

        public virtual Task<Message> SendTextMessageAsync(ChatId chatId,
            string text,
            ParseMode parseMode = ParseMode.Default,
            bool disableWebPagePreview = false,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .SendTextMessageAsync(chatId,
                    text,
                    parseMode,
                    disableWebPagePreview,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken);

        public virtual Task<Message> ForwardMessageAsync(ChatId chatId,
            ChatId fromChatId,
            int messageId,
            bool disableNotification = false,
            CancellationToken cancellationToken = default)
            => Bot
                .ForwardMessageAsync(chatId,
                    fromChatId,
                    messageId,
                    disableNotification,
                    cancellationToken);

        public virtual Task<Message> SendPhotoAsync(ChatId chatId,
            InputOnlineFile photo,
            string? caption = null,
            ParseMode parseMode = ParseMode.Default,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .SendPhotoAsync(chatId,
                    photo,
                    caption,
                    parseMode,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken);

        public virtual Task<Message> SendAudioAsync(ChatId chatId,
            InputOnlineFile audio,
            string? caption = null,
            ParseMode parseMode = ParseMode.Default,
            int duration = 0,
            string? performer = null,
            string? title = null,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default,
            InputMedia? thumb = null)
            => Bot
                .SendAudioAsync(chatId,
                    audio,
                    caption,
                    parseMode,
                    duration,
                    performer,
                    title,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken,
                    thumb);

        public virtual Task<Message> SendDocumentAsync(ChatId chatId,
            InputOnlineFile document,
            string? caption = null,
            ParseMode parseMode = ParseMode.Default,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default,
            InputMedia? thumb = null)
            => Bot
                .SendDocumentAsync(chatId,
                    document,
                    caption,
                    parseMode,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken,
                    thumb);

        public virtual Task<Message> SendStickerAsync(ChatId chatId,
            InputOnlineFile? sticker,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .SendStickerAsync(chatId,
                    sticker,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken);

        public virtual Task<Message> SendVideoAsync(ChatId chatId,
            InputOnlineFile video,
            int duration = 0,
            int width = 0,
            int height = 0,
            string? caption = null,
            ParseMode parseMode = ParseMode.Default,
            bool supportsStreaming = false,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default,
            InputMedia? thumb = null)
            => Bot
                .SendVideoAsync(chatId,
                    video,
                    duration,
                    width,
                    height,
                    caption,
                    parseMode,
                    supportsStreaming,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken,
                    thumb);

        public virtual Task<Message> SendAnimationAsync(ChatId chatId,
            InputOnlineFile animation,
            int duration = 0,
            int width = 0,
            int height = 0,
            InputMedia? thumb = null,
            string? caption = null,
            ParseMode parseMode = ParseMode.Default,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .SendAnimationAsync(chatId,
                    animation,
                    duration,
                    width,
                    height,
                    thumb,
                    caption,
                    parseMode,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken);

        public virtual Task<Message> SendVoiceAsync(ChatId chatId,
            InputOnlineFile voice,
            string? caption = null,
            ParseMode parseMode = ParseMode.Default,
            int duration = 0,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .SendVoiceAsync(chatId,
                    voice,
                    caption,
                    parseMode,
                    duration,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken);

        public virtual Task<Message> SendVideoNoteAsync(ChatId chatId,
            InputTelegramFile? videoNote,
            int duration = 0,
            int length = 0,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default,
            InputMedia? thumb = null)
            => Bot
                .SendVideoNoteAsync(chatId,
                    videoNote,
                    duration,
                    length,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken,
                    thumb);

        public virtual Task<Message[]> SendMediaGroupAsync(ChatId chatId,
            IEnumerable<InputMediaBase> media,
            bool disableNotification = false,
            int replyToMessageId = 0,
            CancellationToken cancellationToken = default)
            => Bot
                .SendMediaGroupAsync(chatId,
                    media,
                    disableNotification,
                    replyToMessageId,
                    cancellationToken);

        public virtual Task<Message[]> SendMediaGroupAsync(IEnumerable<IAlbumInputMedia> inputMedia,
            ChatId chatId,
            bool disableNotification = false,
            int replyToMessageId = 0,
            CancellationToken cancellationToken = default)
            => Bot
                .SendMediaGroupAsync(inputMedia,
                    chatId,
                    disableNotification,
                    replyToMessageId,
                    cancellationToken);

        public virtual Task<Message> SendLocationAsync(ChatId chatId,
            float latitude,
            float longitude,
            int livePeriod = 0,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .SendLocationAsync(chatId,
                    latitude,
                    longitude,
                    livePeriod,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken);

        public virtual Task<Message> SendVenueAsync(ChatId chatId,
            float latitude,
            float longitude,
            string title,
            string address,
            string? foursquareId = null,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default,
            string? foursquareType = null)
            => Bot
                .SendVenueAsync(chatId,
                    latitude,
                    longitude,
                    title,
                    address,
                    foursquareId,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken,
                    foursquareType);

        public virtual Task<Message> SendContactAsync(ChatId chatId,
            string phoneNumber,
            string firstName,
            string? lastName = null,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default,
            string? vCard = null)
            => Bot
                .SendContactAsync(chatId,
                    phoneNumber,
                    firstName,
                    lastName,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken,
                    vCard);

        public virtual Task<Message> SendPollAsync(ChatId chatId,
            string question,
            IEnumerable<string> options,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default,
            bool? isAnonymous = null,
            PollType? type = null,
            bool? allowsMultipleAnswers = null,
            int? correctOptionId = null,
            bool? isClosed = null,
            string? explanation = null,
            ParseMode explanationParseMode = ParseMode.Default,
            int? openPeriod = null,
            DateTime? closeDate = null)
            => Bot
                .SendPollAsync(chatId,
                    question,
                    options,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken,
                    isAnonymous,
                    type,
                    allowsMultipleAnswers,
                    correctOptionId,
                    isClosed,
                    explanation,
                    explanationParseMode,
                    openPeriod,
                    closeDate);

        public virtual Task<Message> SendDiceAsync(ChatId chatId,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default,
            Emoji? emoji = null)
            => Bot
                .SendDiceAsync(chatId,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken,
                    emoji);

        public virtual Task SendChatActionAsync(ChatId chatId,
            ChatAction chatAction,
            CancellationToken cancellationToken = default)
            => Bot
                .SendChatActionAsync(chatId,
                    chatAction,
                    cancellationToken);

        public virtual Task<UserProfilePhotos> GetUserProfilePhotosAsync(int userId,
            int offset = 0,
            int limit = 0,
            CancellationToken cancellationToken = default)
            => Bot
                .GetUserProfilePhotosAsync(userId,
                    offset,
                    limit,
                    cancellationToken);

        public virtual Task<File> GetFileAsync(string fileId,
            CancellationToken cancellationToken = default)
            => Bot
                .GetFileAsync(fileId,
                    cancellationToken);

        public virtual Task<Stream> DownloadFileAsync(string filePath,
            CancellationToken cancellationToken = default)
            => Bot
                .DownloadFileAsync(filePath,
                    cancellationToken);

        public virtual Task DownloadFileAsync(string filePath,
            Stream destination,
            CancellationToken cancellationToken = default)
            => Bot
                .DownloadFileAsync(filePath,
                    destination,
                    cancellationToken);

        public virtual Task<File> GetInfoAndDownloadFileAsync(string fileId,
            Stream destination,
            CancellationToken cancellationToken = default)
            => Bot
                .GetInfoAndDownloadFileAsync(fileId,
                    destination,
                    cancellationToken);

        public virtual Task KickChatMemberAsync(ChatId chatId,
            int userId,
            DateTime untilDate = default,
            CancellationToken cancellationToken = default)
            => Bot
                .KickChatMemberAsync(chatId,
                    userId,
                    untilDate,
                    cancellationToken);

        public virtual Task LeaveChatAsync(ChatId chatId,
            CancellationToken cancellationToken = default)
            => Bot
                .LeaveChatAsync(chatId,
                    cancellationToken);

        public virtual Task UnbanChatMemberAsync(ChatId chatId,
            int userId,
            CancellationToken cancellationToken = default)
            => Bot
                .UnbanChatMemberAsync(chatId,
                    userId,
                    cancellationToken);

        public virtual Task<Chat> GetChatAsync(ChatId chatId,
            CancellationToken cancellationToken = default)
            => Bot
                .GetChatAsync(chatId,
                    cancellationToken);

        public virtual Task<ChatMember[]> GetChatAdministratorsAsync(ChatId chatId,
            CancellationToken cancellationToken = default)
            => Bot
                .GetChatAdministratorsAsync(chatId,
                    cancellationToken);

        public virtual Task<int> GetChatMembersCountAsync(ChatId chatId,
            CancellationToken cancellationToken = default)
            => Bot
                .GetChatMembersCountAsync(chatId,
                    cancellationToken);

        public virtual Task<ChatMember> GetChatMemberAsync(ChatId chatId,
            int userId,
            CancellationToken cancellationToken = default)
            => Bot
                .GetChatMemberAsync(chatId,
                    userId,
                    cancellationToken);

        public virtual Task AnswerCallbackQueryAsync(string callbackQueryId,
            string? text = null,
            bool showAlert = false,
            string? url = null,
            int cacheTime = 0,
            CancellationToken cancellationToken = default)
            => Bot
                .AnswerCallbackQueryAsync(callbackQueryId,
                    text,
                    showAlert,
                    url,
                    cacheTime,
                    cancellationToken);

        public virtual Task RestrictChatMemberAsync(ChatId chatId,
            int userId,
            ChatPermissions permissions,
            DateTime untilDate = default,
            CancellationToken cancellationToken = default)
            => Bot
                .RestrictChatMemberAsync(chatId,
                    userId,
                    permissions,
                    untilDate,
                    cancellationToken);

        public virtual Task PromoteChatMemberAsync(ChatId chatId,
            int userId,
            bool? canChangeInfo = null,
            bool? canPostMessages = null,
            bool? canEditMessages = null,
            bool? canDeleteMessages = null,
            bool? canInviteUsers = null,
            bool? canRestrictMembers = null,
            bool? canPinMessages = null,
            bool? canPromoteMembers = null,
            CancellationToken cancellationToken = default)
            => Bot
                .PromoteChatMemberAsync(chatId,
                    userId,
                    canChangeInfo,
                    canPostMessages,
                    canEditMessages,
                    canDeleteMessages,
                    canInviteUsers,
                    canRestrictMembers,
                    canPinMessages,
                    canPromoteMembers,
                    cancellationToken);

        public virtual Task SetChatAdministratorCustomTitleAsync(ChatId chatId,
            int userId,
            string customTitle,
            CancellationToken cancellationToken = default)
            => Bot
                .SetChatAdministratorCustomTitleAsync(chatId,
                    userId,
                    customTitle,
                    cancellationToken);

        public virtual Task SetChatPermissionsAsync(ChatId chatId,
            ChatPermissions permissions,
            CancellationToken cancellationToken = default)
            => Bot
                .SetChatPermissionsAsync(chatId,
                    permissions,
                    cancellationToken);

        public virtual Task<BotCommand[]> GetMyCommandsAsync(CancellationToken cancellationToken = default)
            => Bot
                .GetMyCommandsAsync(cancellationToken);

        public virtual Task SetMyCommandsAsync(IEnumerable<BotCommand> commands,
            CancellationToken cancellationToken = default)
            => Bot
                .SetMyCommandsAsync(commands,
                    cancellationToken);

        public virtual Task<Message> EditMessageTextAsync(ChatId chatId,
            int messageId,
            string text,
            ParseMode parseMode = ParseMode.Default,
            bool disableWebPagePreview = false,
            InlineKeyboardMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .EditMessageTextAsync(chatId,
                    messageId,
                    text,
                    parseMode,
                    disableWebPagePreview,
                    replyMarkup,
                    cancellationToken);

        public virtual Task EditMessageTextAsync(string inlineMessageId,
            string text,
            ParseMode parseMode = ParseMode.Default,
            bool disableWebPagePreview = false,
            InlineKeyboardMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .EditMessageTextAsync(inlineMessageId,
                    text,
                    parseMode,
                    disableWebPagePreview,
                    replyMarkup,
                    cancellationToken);

        public virtual Task<Message> StopMessageLiveLocationAsync(ChatId chatId,
            int messageId,
            InlineKeyboardMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .StopMessageLiveLocationAsync(chatId,
                    messageId,
                    replyMarkup,
                    cancellationToken);

        public virtual Task StopMessageLiveLocationAsync(string inlineMessageId,
            InlineKeyboardMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .StopMessageLiveLocationAsync(inlineMessageId,
                    replyMarkup,
                    cancellationToken);

        public virtual Task<Message> EditMessageCaptionAsync(ChatId chatId,
            int messageId,
            string caption,
            InlineKeyboardMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default,
            ParseMode parseMode = ParseMode.Default)
            => Bot
                .EditMessageCaptionAsync(chatId,
                    messageId,
                    caption,
                    replyMarkup,
                    cancellationToken,
                    parseMode);

        public virtual Task EditMessageCaptionAsync(string inlineMessageId,
            string caption,
            InlineKeyboardMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default,
            ParseMode parseMode = ParseMode.Default)
            => Bot
                .EditMessageCaptionAsync(inlineMessageId,
                    caption,
                    replyMarkup,
                    cancellationToken,
                    parseMode);

        public virtual Task<Message> EditMessageMediaAsync(ChatId chatId,
            int messageId,
            InputMediaBase media,
            InlineKeyboardMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .EditMessageMediaAsync(chatId,
                    messageId,
                    media,
                    replyMarkup,
                    cancellationToken);

        public virtual Task EditMessageMediaAsync(string inlineMessageId,
            InputMediaBase media,
            InlineKeyboardMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .EditMessageMediaAsync(inlineMessageId,
                    media,
                    replyMarkup,
                    cancellationToken);

        public virtual Task<Message> EditMessageReplyMarkupAsync(ChatId chatId,
            int messageId,
            InlineKeyboardMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .EditMessageReplyMarkupAsync(chatId,
                    messageId,
                    replyMarkup,
                    cancellationToken);

        public virtual Task EditMessageReplyMarkupAsync(string inlineMessageId,
            InlineKeyboardMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .EditMessageReplyMarkupAsync(inlineMessageId,
                    replyMarkup,
                    cancellationToken);

        public virtual Task<Message> EditMessageLiveLocationAsync(ChatId chatId,
            int messageId,
            float latitude,
            float longitude,
            InlineKeyboardMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .EditMessageLiveLocationAsync(chatId,
                    messageId,
                    latitude,
                    longitude,
                    replyMarkup,
                    cancellationToken);

        public virtual Task EditMessageLiveLocationAsync(string inlineMessageId,
            float latitude,
            float longitude,
            InlineKeyboardMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .EditMessageLiveLocationAsync(inlineMessageId,
                    latitude,
                    longitude,
                    replyMarkup,
                    cancellationToken);

        public virtual Task<Poll> StopPollAsync(ChatId chatId,
            int messageId,
            InlineKeyboardMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .StopPollAsync(chatId,
                    messageId,
                    replyMarkup,
                    cancellationToken);

        public virtual Task DeleteMessageAsync(ChatId chatId,
            int messageId,
            CancellationToken cancellationToken = default)
            => Bot
                .DeleteMessageAsync(chatId,
                    messageId,
                    cancellationToken);

        public virtual Task AnswerInlineQueryAsync(string inlineQueryId,
            IEnumerable<InlineQueryResultBase> results,
            int? cacheTime = null,
            bool isPersonal = false,
            string? nextOffset = null,
            string? switchPmText = null,
            string? switchPmParameter = null,
            CancellationToken cancellationToken = default)
            => Bot
                .AnswerInlineQueryAsync(inlineQueryId,
                    results,
                    cacheTime,
                    isPersonal,
                    nextOffset,
                    switchPmText,
                    switchPmParameter,
                    cancellationToken);

        public virtual Task<Message> SendInvoiceAsync(int chatId,
            string title,
            string description,
            string payload,
            string providerToken,
            string startParameter,
            string currency,
            IEnumerable<LabeledPrice> prices,
            string? providerData = null,
            string? photoUrl = null,
            int photoSize = 0,
            int photoWidth = 0,
            int photoHeight = 0,
            bool needName = false,
            bool needPhoneNumber = false,
            bool needEmail = false,
            bool needShippingAddress = false,
            bool isFlexible = false,
            bool disableNotification = false,
            int replyToMessageId = 0,
            InlineKeyboardMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default,
            bool sendPhoneNumberToProvider = false,
            bool sendEmailToProvider = false)
            => Bot
                .SendInvoiceAsync(chatId,
                    title,
                    description,
                    payload,
                    providerToken,
                    startParameter,
                    currency,
                    prices,
                    providerData,
                    photoUrl,
                    photoSize,
                    photoWidth,
                    photoHeight,
                    needName,
                    needPhoneNumber,
                    needEmail,
                    needShippingAddress,
                    isFlexible,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken,
                    sendPhoneNumberToProvider,
                    sendEmailToProvider);

        public virtual Task AnswerShippingQueryAsync(string shippingQueryId,
            IEnumerable<ShippingOption> shippingOptions,
            CancellationToken cancellationToken = default)
            => Bot
                .AnswerShippingQueryAsync(shippingQueryId,
                    shippingOptions,
                    cancellationToken);

        public virtual Task AnswerShippingQueryAsync(string shippingQueryId,
            string errorMessage,
            CancellationToken cancellationToken = default)
            => Bot
                .AnswerShippingQueryAsync(shippingQueryId,
                    errorMessage,
                    cancellationToken);

        public virtual Task AnswerPreCheckoutQueryAsync(string preCheckoutQueryId,
            CancellationToken cancellationToken = default)
            => Bot
                .AnswerPreCheckoutQueryAsync(preCheckoutQueryId,
                    cancellationToken);

        public virtual Task AnswerPreCheckoutQueryAsync(string preCheckoutQueryId,
            string errorMessage,
            CancellationToken cancellationToken = default)
            => Bot
                .AnswerPreCheckoutQueryAsync(preCheckoutQueryId,
                    errorMessage,
                    cancellationToken);

        public virtual Task<Message> SendGameAsync(long chatId,
            string gameShortName,
            bool disableNotification = false,
            int replyToMessageId = 0,
            InlineKeyboardMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
            => Bot
                .SendGameAsync(chatId,
                    gameShortName,
                    disableNotification,
                    replyToMessageId,
                    replyMarkup,
                    cancellationToken);

        public virtual Task<Message> SetGameScoreAsync(int userId,
            int score,
            long chatId,
            int messageId,
            bool force = false,
            bool disableEditMessage = false,
            CancellationToken cancellationToken = default)
            => Bot
                .SetGameScoreAsync(userId,
                    score,
                    chatId,
                    messageId,
                    force,
                    disableEditMessage,
                    cancellationToken);

        public virtual Task SetGameScoreAsync(int userId,
            int score,
            string inlineMessageId,
            bool force = false,
            bool disableEditMessage = false,
            CancellationToken cancellationToken = default)
            => Bot
                .SetGameScoreAsync(userId,
                    score,
                    inlineMessageId,
                    force,
                    disableEditMessage,
                    cancellationToken);

        public virtual Task<GameHighScore[]> GetGameHighScoresAsync(int userId,
            long chatId,
            int messageId,
            CancellationToken cancellationToken = default)
            => Bot
                .GetGameHighScoresAsync(userId,
                    chatId,
                    messageId,
                    cancellationToken);

        public virtual Task<GameHighScore[]> GetGameHighScoresAsync(int userId,
            string inlineMessageId,
            CancellationToken cancellationToken = default)
            => Bot
                .GetGameHighScoresAsync(userId,
                    inlineMessageId,
                    cancellationToken);

        public virtual Task<StickerSet> GetStickerSetAsync(string name,
            CancellationToken cancellationToken = default)
            => Bot
                .GetStickerSetAsync(name,
                    cancellationToken);

        public virtual Task<File> UploadStickerFileAsync(int userId,
            InputFileStream pngSticker,
            CancellationToken cancellationToken = default)
            => Bot
                .UploadStickerFileAsync(userId,
                    pngSticker,
                    cancellationToken);

        public virtual Task CreateNewStickerSetAsync(int userId,
            string name,
            string title,
            InputOnlineFile pngSticker,
            string emojis,
            bool isMasks = false,
            MaskPosition? maskPosition = null,
            CancellationToken cancellationToken = default)
            => Bot
                .CreateNewStickerSetAsync(userId,
                    name,
                    title,
                    pngSticker,
                    emojis,
                    isMasks,
                    maskPosition,
                    cancellationToken);

        public virtual Task AddStickerToSetAsync(int userId,
            string name,
            InputOnlineFile pngSticker,
            string emojis,
            MaskPosition? maskPosition = null,
            CancellationToken cancellationToken = default)
            => Bot
                .AddStickerToSetAsync(userId,
                    name,
                    pngSticker,
                    emojis,
                    maskPosition,
                    cancellationToken);

        public virtual Task CreateNewAnimatedStickerSetAsync(int userId,
            string name,
            string title,
            InputFileStream tgsSticker,
            string emojis,
            bool isMasks = false,
            MaskPosition? maskPosition = null,
            CancellationToken cancellationToken = default)
            => Bot
                .CreateNewAnimatedStickerSetAsync(userId,
                    name,
                    title,
                    tgsSticker,
                    emojis,
                    isMasks,
                    maskPosition,
                    cancellationToken);

        public virtual Task AddAnimatedStickerToSetAsync(int userId,
            string name,
            InputFileStream tgsSticker,
            string emojis,
            MaskPosition? maskPosition = null,
            CancellationToken cancellationToken = default)
            => Bot
                .AddAnimatedStickerToSetAsync(userId,
                    name,
                    tgsSticker,
                    emojis,
                    maskPosition,
                    cancellationToken);

        public virtual Task SetStickerPositionInSetAsync(string sticker,
            int position,
            CancellationToken cancellationToken = default)
            => Bot
                .SetStickerPositionInSetAsync(sticker,
                    position,
                    cancellationToken);

        public virtual Task DeleteStickerFromSetAsync(string sticker,
            CancellationToken cancellationToken = default)
            => Bot
                .DeleteStickerFromSetAsync(sticker,
                    cancellationToken);

        public virtual Task SetStickerSetThumbAsync(string name,
            int userId,
            InputOnlineFile? thumb = null,
            CancellationToken cancellationToken = default)
            => Bot
                .SetStickerSetThumbAsync(name,
                    userId,
                    thumb,
                    cancellationToken);

        public virtual Task<string> ExportChatInviteLinkAsync(ChatId chatId,
            CancellationToken cancellationToken = default)
            => Bot
                .ExportChatInviteLinkAsync(chatId,
                    cancellationToken);

        public virtual Task SetChatPhotoAsync(ChatId chatId,
            InputFileStream photo,
            CancellationToken cancellationToken = default)
            => Bot
                .SetChatPhotoAsync(chatId,
                    photo,
                    cancellationToken);

        public virtual Task DeleteChatPhotoAsync(ChatId chatId,
            CancellationToken cancellationToken = default)
            => Bot
                .DeleteChatPhotoAsync(chatId,
                    cancellationToken);

        public virtual Task SetChatTitleAsync(ChatId chatId,
            string title,
            CancellationToken cancellationToken = default)
            => Bot
                .SetChatTitleAsync(chatId,
                    title,
                    cancellationToken);

        public virtual Task SetChatDescriptionAsync(ChatId chatId,
            string? description = null,
            CancellationToken cancellationToken = default)
            => Bot
                .SetChatDescriptionAsync(chatId,
                    description,
                    cancellationToken);

        public virtual Task PinChatMessageAsync(ChatId chatId,
            int messageId,
            bool disableNotification = false,
            CancellationToken cancellationToken = default)
            => Bot
                .PinChatMessageAsync(chatId,
                    messageId,
                    disableNotification,
                    cancellationToken);

        public virtual Task UnpinChatMessageAsync(ChatId chatId,
            CancellationToken cancellationToken = default)
            => Bot
                .UnpinChatMessageAsync(chatId,
                    cancellationToken);

        public virtual Task SetChatStickerSetAsync(ChatId chatId,
            string stickerSetName,
            CancellationToken cancellationToken = default)
            => Bot
                .SetChatStickerSetAsync(chatId,
                    stickerSetName,
                    cancellationToken);

        public Task DeleteChatStickerSetAsync(ChatId chatId,
            CancellationToken cancellationToken = default)
            => Bot
                .DeleteChatStickerSetAsync(chatId,
                    cancellationToken);

        public int BotId => Bot.BotId;

        public TimeSpan Timeout
        {
            get => Bot.Timeout;
            set => Bot.Timeout = value;
        }

        public bool IsReceiving => Bot.IsReceiving;

        public int MessageOffset
        {
            get => Bot.MessageOffset;
            set => Bot.MessageOffset = value;
        }

        public event EventHandler<ApiRequestEventArgs>? MakingApiRequest;
        public event EventHandler<ApiResponseEventArgs>? ApiResponseReceived;
        public event EventHandler<UpdateEventArgs>? OnUpdate;
        public event EventHandler<MessageEventArgs>? OnMessage;
        public event EventHandler<MessageEventArgs>? OnMessageEdited;
        public event EventHandler<InlineQueryEventArgs>? OnInlineQuery;
        public event EventHandler<ChosenInlineResultEventArgs>? OnInlineResultChosen;
        public event EventHandler<CallbackQueryEventArgs>? OnCallbackQuery;
        public event EventHandler<ReceiveErrorEventArgs>? OnReceiveError;
        public event EventHandler<ReceiveGeneralErrorEventArgs>? OnReceiveGeneralError;
    }
}