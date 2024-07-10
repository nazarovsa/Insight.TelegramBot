using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Insight.TelegramBot
{
    public class Bot : IBot
    {
        protected ITelegramBotClient Client { get; private set; }

        public Bot(ITelegramBotClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public virtual Task<Message> SendMessageAsync(TextMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendTextMessageAsync(
                message.ChatId,
                message.Text,
                message.MessageThreadId,
                message.ParseMode,
                message.Entities,
                message.LinkPreviewOptions,
                message.DisableNotification,
                message.ProtectContent,
                message.MessageEffectId,
                message.ReplyParameters,
                message.ReplyMarkup,
                message.BusinessConnectionId,
                cancellationToken);
        }

        public virtual Task<Message> SendDocumentAsync(DocumentMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendDocumentAsync(
                message.ChatId,
                message.InputFile,
                message.MessageThreadId,
                message.Thumbnail,
                message.Caption,
                message.ParseMode,
                message.Entities,
                message.DisableContentTypeDetection,
                message.DisableNotification,
                message.ProtectContent,
                message.MessageEffectId,
                message.ReplyParameters,
                message.ReplyMarkup,
                message.BusinessConnectionId,
                cancellationToken);
        }

        public virtual Task<Message> SendPhotoAsync(PhotoMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendPhotoAsync(
                message.ChatId,
                message.InputFile,
                message.MessageThreadId,
                message.Caption,
                message.ParseMode,
                message.Entities,
                message.ShowCaptionAboveMedia,
                message.HasSpoiler,
                message.DisableNotification,
                message.ProtectContent,
                message.MessageEffectId,
                message.ReplyParameters,
                message.ReplyMarkup,
                message.BusinessConnectionId,
                cancellationToken);
        }

        public virtual Task<Message> SendAudioAsync(AudioMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendAudioAsync(
                message.ChatId,
                message.InputFile,
                message.MessageThreadId,
                message.Caption,
                message.ParseMode,
                message.Entities,
                message.Duration,
                message.Performer,
                message.Title,
                message.Thumbnail,
                message.DisableNotification,
                message.ProtectContent,
                message.MessageEffectId,
                message.ReplyParameters,
                message.ReplyMarkup,
                message.BusinessConnectionId,
                cancellationToken);
        }

        public virtual Task<Message> SendAnimationAsync(AnimationMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendAnimationAsync(
                message.ChatId,
                message.InputFile,
                message.MessageThreadId,
                message.Duration,
                message.Width,
                message.Height,
                message.Thumbnail,
                message.Caption,
                message.ParseMode,
                message.Entities,
                message.ShowCaptionAboveMedia,
                message.HasSpoiler,
                message.DisableNotification,
                message.ProtectContent,
                message.MessageEffectId,
                message.ReplyParameters,
                message.ReplyMarkup,
                message.BusinessConnectionId,
                cancellationToken);
        }

        public virtual Task<Message> SendStickerAsync(StickerMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendStickerAsync(
                message.ChatId,
                message.InputFile,
                message.MessageThreadId,
                message.Emoji,
                message.DisableNotification,
                message.ProtectContent,
                message.MessageEffectId,
                message.ReplyParameters,
                message.ReplyMarkup,
                message.BusinessConnectionId, cancellationToken);
        }

        public virtual Task<Message> SendDiceAsync(DiceMessage message, CancellationToken cancellationToken = default)
        {
            return Client.SendDiceAsync(message.ChatId,
                message.MessageThreadId,
                message.Emoji,
                message.DisableNotification,
                message.ProtectContent,
                message.MessageEffectId,
                message.ReplyParameters,
                message.ReplyMarkup,
                message.BusinessConnectionId,
                cancellationToken);
        }

        public virtual Task<Message> SendGameAsync(GameMessage message, CancellationToken cancellationToken = default)
        {
            if (message.ChatId.Identifier == null)
            {
                throw new ArgumentNullException(nameof(message.ChatId.Identifier));
            }

            return Client.SendGameAsync(
                message.ChatId.Identifier.Value,
                message.GameShortName,
                message.MessageThreadId, message.DisableNotification,
                message.ProtectContent,
                message.MessageEffectId,
                message.ReplyParameters,
                message.ReplyMarkup,
                message.BusinessConnectionId,
                cancellationToken);
        }

        public virtual Task<Message> SendLocationAsync(LocationMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendLocationAsync(
                message.ChatId,
                message.Latitude,
                message.Longitude,
                message.MessageThreadId,
                message.HorizontalAccuracy,
                message.LivePeriod,
                message.Heading,
                message.ProximityAlertRadius,
                message.DisableNotification,
                message.ProtectContent,
                message.MessageEffectId,
                message.ReplyParameters,
                message.ReplyMarkup,
                message.BusinessConnectionId,
                cancellationToken);
        }

        public virtual Task<Message> SendVideoAsync(VideoMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendVideoAsync(message.ChatId, 
                message.InputFile,
                message.MessageThreadId,
                message.Duration, message.Width,
                message.Height,
                message.Thumbnail,
                message.Caption,
                message.ParseMode,
                message.Entities,
                message.ShowCaptionAboveMedia,
                message.HasSpoiler,
                message.SupportsStreaming,
                message.DisableNotification,
                message.ProtectContent,
                message.MessageEffectId,
                message.ReplyParameters,
                message.ReplyMarkup, 
                message.BusinessConnectionId, 
                cancellationToken);
        }

        public virtual Task<Message> SendVoiceAsync(VoiceMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendVoiceAsync(message.ChatId, 
                message.InputFile,
                message.MessageThreadId,
                message.Caption,
                message.ParseMode,
                message.Entities,
                message.Duration, 
                message.DisableNotification, 
                message.ProtectContent,
                message.MessageEffectId,
                message.ReplyParameters,
                message.ReplyMarkup,
                message.BusinessConnectionId,
                cancellationToken);
        }

        public virtual Task SendChatActionAsync(ChatId chatId,
            ChatAction action,
            int? messageThreadId = null,
            string? businessConnectionId = null,
            CancellationToken cancellationToken = default)
        {
            return Client.SendChatActionAsync(chatId, action, messageThreadId, businessConnectionId, cancellationToken);
        }

        public virtual Task<Message> SendVideoNoteAsync(VideoNoteMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendVideoNoteAsync(message.ChatId, 
                message.InputFile, 
                message.MessageThreadId,
                message.Duration, message.Length,
                message.Thumbnail,
                message.DisableNotification,
                message.ProtectContent, 
                message.MessageEffectId,
                message.ReplyParameters,
                message.ReplyMarkup, 
                message.BusinessConnectionId, 
                cancellationToken);
        }

        public virtual Task<Message> SendContactAsync(ContactMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendContactAsync(message.ChatId, 
                message.PhoneNumber,
                message.FirstName,
                message.MessageThreadId,
                message.LastName,
                message.VCard, 
                message.DisableNotification, 
                message.ProtectContent, 
                message.MessageEffectId,
                message.ReplyParameters,
                message.ReplyMarkup, 
                message.BusinessConnectionId, 
                cancellationToken);
        }

        public virtual Task<Message> EditMessageTextAsync(int messageId,
            TextMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.EditMessageTextAsync(message.ChatId,
                messageId,
                message.Text,
                message.ParseMode,
                message.Entities,
                message.LinkPreviewOptions, 
                message.ReplyMarkup as InlineKeyboardMarkup,
                message.BusinessConnectionId,
                cancellationToken);
        }

        public virtual Task DeleteChatPhotoAsync(ChatId chatId, CancellationToken cancellationToken = default)
        {
            return Client.DeleteChatPhotoAsync(chatId, cancellationToken);
        }

        public virtual Task DeleteChatStickerSetAsync(ChatId chatId, CancellationToken cancellationToken = default)
        {
            return Client.DeleteChatStickerSetAsync(chatId, cancellationToken);
        }

        public virtual Task<Message> ForwardMessageAsync(ChatId receiverId,
            ChatId chatId,
            int messageId,
            int? messageThreadId = default,
            bool disableNotification = false,
            bool protectContent = false,
            CancellationToken cancellationToken = default)
        {
            return Client.ForwardMessageAsync(receiverId, chatId, messageId, messageThreadId, disableNotification,
                protectContent, cancellationToken);
        }

        public virtual Task LeaveChatAsync(long chatId, CancellationToken cancellationToken = default)
        {
            return Client.LeaveChatAsync(chatId, cancellationToken);
        }

        public virtual Task DeleteMessageAsync(long chatId,
            int messageId,
            CancellationToken cancellationToken = default)
        {
            return Client.DeleteMessageAsync(chatId, messageId, cancellationToken);
        }

        public virtual Task<ChatFullInfo> GetChatAsync(ChatId id, CancellationToken cancellationToken = default)
        {
            return Client.GetChatAsync(id, cancellationToken);
        }

        public virtual Task<int> GetChatMembersCountAsync(ChatId id, CancellationToken cancellationToken = default)
        {
            return Client.GetChatMemberCountAsync(id, cancellationToken);
        }

        public virtual Task<User> GetMeAsync(CancellationToken cancellationToken = default)
        {
            return Client.GetMeAsync(cancellationToken);
        }

        public virtual Task<ChatMember[]> GetChatAdministratorsAsync(ChatId id,
            CancellationToken cancellationToken = default)
        {
            return Client.GetChatAdministratorsAsync(id, cancellationToken);
        }

        public virtual Task<File> GetFileAsync(string id, CancellationToken cancellationToken = default)
        {
            return Client.GetFileAsync(id, cancellationToken);
        }

        public virtual Task<ChatMember> GetChatMemberAsync(ChatId chatId,
            long userId,
            CancellationToken cancellationToken = default)
        {
            return Client.GetChatMemberAsync(chatId, userId, cancellationToken);
        }

        public virtual Task<BotCommand[]> GetMyCommandsAsync(BotCommandScope? scope = null, string? languageCode = null,
            CancellationToken cancellationToken = default)
        {
            return Client.GetMyCommandsAsync(scope, languageCode, cancellationToken);
        }

        public virtual Task<StickerSet> GetStickerSetAsync(string name, CancellationToken cancellationToken = default)
        {
            return Client.GetStickerSetAsync(name, cancellationToken);
        }

        public virtual Task<UserProfilePhotos> GetUserProfilePhotosAsync(int userId,
            int offset = 0,
            int limit = 0,
            CancellationToken cancellationToken = default)
        {
            return Client.GetUserProfilePhotosAsync(userId, offset, limit, cancellationToken);
        }
    }
}