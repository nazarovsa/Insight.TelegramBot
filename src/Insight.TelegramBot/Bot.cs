using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Insight.TelegramBot
{
    public class Bot : IBot
    {
        protected ITelegramBotClient Client { get; private set; }

        public Bot(ITelegramBotClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            Client = client;
        }

        public virtual Task<Message> SendMessageAsync(TextMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendTextMessageAsync(message.ChatId, message.Text, message.ParseMode,
                message.DisableWebPagePreview, message.DisableNotification, message.ReplyToMessageId,
                message.ReplyMarkup, cancellationToken);
        }

        public virtual Task<Message> SendDocumentAsync(DocumentMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendDocumentAsync(message.ChatId, message.InputOnlineFile, message.Caption, message.ParseMode,
                message.DisableNotification, message.ReplyToMessageId, message.ReplyMarkup, cancellationToken);
        }

        public virtual Task<Message> SendPhotoAsync(PhotoMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendPhotoAsync(message.ChatId, message.InputOnlineFile, message.Caption, message.ParseMode,
                message.DisableNotification, message.ReplyToMessageId, message.ReplyMarkup, cancellationToken);
        }

        public virtual Task<Message> SendAudioAsync(AudioMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendAudioAsync(message.ChatId, message.InputOnlineFile, message.Caption, message.ParseMode,
                message.Duration, message.Performer, message.Title,
                message.DisableNotification, message.ReplyToMessageId, message.ReplyMarkup, cancellationToken);
        }

        public virtual Task<Message> SendAnimationAsync(AnimationMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendAnimationAsync(message.ChatId, message.InputOnlineFile, message.Duration, message.Width,
                message.Height, message.Thumb, message.Caption, message.ParseMode,
                message.DisableNotification, message.ReplyToMessageId, message.ReplyMarkup, cancellationToken);
        }

        public virtual Task<Message> SendStickerAsync(StickerMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendStickerAsync(message.ChatId, message.InputOnlineFile,
                message.DisableNotification, message.ReplyToMessageId, message.ReplyMarkup, cancellationToken);
        }

        public virtual Task<Message> SendDiceAsync(DiceMessage message, CancellationToken cancellationToken = default)
        {
            return Client.SendDiceAsync(message.ChatId,
                message.DisableNotification, message.ReplyToMessageId, message.ReplyMarkup, cancellationToken,
                message.Emoji);
        }

        public virtual Task<Message> SendGameAsync(GameMessage message, CancellationToken cancellationToken = default)
        {
            return Client.SendGameAsync(message.ChatId.Identifier, message.GameShortName,
                message.DisableNotification, message.ReplyToMessageId, message.ReplyMarkup, cancellationToken);
        }

        public virtual Task<Message> SendLocationAsync(LocationMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendLocationAsync(message.ChatId, message.Latitude, message.Longitude, message.LivePeriod,
                message.DisableNotification, message.ReplyToMessageId, message.ReplyMarkup, cancellationToken);
        }

        public virtual Task<Message> SendVideoAsync(VideoMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendVideoAsync(message.ChatId, message.InputOnlineFile, message.Duration, message.Width, message.Height, message.Caption, message.ParseMode, message.SupportsStreaming,
                message.DisableNotification, message.ReplyToMessageId, message.ReplyMarkup, cancellationToken);
        }


        public virtual Task<Message> SendVoiceAsync(VoiceMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendVoiceAsync(message.ChatId, message.InputOnlineFile, message.Caption,message.ParseMode, message.Duration,
                message.DisableNotification, message.ReplyToMessageId, message.ReplyMarkup, cancellationToken);
        }

        public virtual Task<Message> ForwardMessageAsync(ChatId receiverId,
            ChatId chatId, int messageId,
            bool disableNotification = false,
            CancellationToken cancellationToken = default)
        {
            return Client.ForwardMessageAsync(receiverId, chatId, messageId, disableNotification, cancellationToken);
        }

        public virtual Task DeleteMessageAsync(long chatId, int messageId,
            CancellationToken cancellationToken = default)
        {
            return Client.DeleteMessageAsync(chatId, messageId, cancellationToken);
        }

        public virtual Task<Chat> GetChatAsync(ChatId id, CancellationToken cancellationToken = default)
        {
            return Client.GetChatAsync(id, cancellationToken);
        }

        public virtual Task<int> GetChatMembersCountAsync(ChatId id, CancellationToken cancellationToken = default)
        {
            return Client.GetChatMembersCountAsync(id, cancellationToken);
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

        public virtual Task<ChatMember> GetChatMemberAsync(ChatId chatId, int userId,
            CancellationToken cancellationToken = default)
        {
            return Client.GetChatMemberAsync(chatId, userId, cancellationToken);
        }

        public virtual Task<BotCommand[]> GetMyCommandsAsync(CancellationToken cancellationToken = default)
        {
            return Client.GetMyCommandsAsync(cancellationToken);
        }

        public virtual Task<StickerSet> GetStickerSetAsync(string name, CancellationToken cancellationToken = default)
        {
            return Client.GetStickerSetAsync(name, cancellationToken);
        }

        public virtual Task<UserProfilePhotos> GetUserProfilePhotosAsync(int userId, int offset = 0, int limit = 0,
            CancellationToken cancellationToken = default)
        {
            return Client.GetUserProfilePhotosAsync(userId, offset, limit, cancellationToken);
        }
    }
}