using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Models;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Insight.TelegramBot
{
    public interface IBot
    {
        Task<Message> SendMessageAsync(TextMessage message, CancellationToken cancellationToken = default);

        Task<Message> SendDocumentAsync(DocumentMessage message,
            CancellationToken cancellationToken = default);

        Task<Message> SendPhotoAsync(PhotoMessage message,
            CancellationToken cancellationToken = default);

        Task<Message> SendAudioAsync(AudioMessage message,
            CancellationToken cancellationToken = default);

        Task<Message> SendAnimationAsync(AnimationMessage message,
            CancellationToken cancellationToken = default);

        Task<Message> SendStickerAsync(StickerMessage message, CancellationToken cancellationToken = default);

        Task<Message> SendDiceAsync(DiceMessage message, CancellationToken cancellationToken = default);

        Task<Message> SendGameAsync(GameMessage message, CancellationToken cancellationToken = default);

        Task<Message> SendLocationAsync(LocationMessage message,
            CancellationToken cancellationToken = default);

        Task<Message> SendVideoAsync(VideoMessage message,
            CancellationToken cancellationToken = default);

        Task<Message> SendVoiceAsync(VoiceMessage message,
            CancellationToken cancellationToken = default);

        Task SendChatActionAsync(ChatId chatId,
            ChatAction chatAction,
            CancellationToken cancellationToken = default);

        Task<Message> SendVideoNoteAsync(VideoNoteMessage message,
            CancellationToken cancellationToken = default);

        Task<Message> SendContactAsync(ContactMessage message,
            CancellationToken cancellationToken = default);

        Task<Message> EditMessageTextAsync(int messageId,
            TextMessage message,
            CancellationToken cancellationToken = default);

        Task DeleteChatPhotoAsync(ChatId chatId, CancellationToken cancellationToken = default);

        Task DeleteChatStickerSetAsync(ChatId chatId, CancellationToken cancellationToken = default);

        Task<Message> ForwardMessageAsync(ChatId receiverId,
            ChatId chatId,
            int messageId,
            bool disableNotification = false,
            bool protectContent = false,
            CancellationToken cancellationToken = default);

        Task LeaveChatAsync(long chatId, CancellationToken cancellationToken = default);

        Task DeleteMessageAsync(long chatId, int messageId, CancellationToken cancellationToken = default);

        Task<Chat> GetChatAsync(ChatId id, CancellationToken cancellationToken = default);

        Task<int> GetChatMembersCountAsync(ChatId id, CancellationToken cancellationToken = default);

        Task<User> GetMeAsync(CancellationToken cancellationToken = default);

        Task<ChatMember[]> GetChatAdministratorsAsync(ChatId id, CancellationToken cancellationToken = default);

        Task<File> GetFileAsync(string id, CancellationToken cancellationToken = default);

        Task<ChatMember> GetChatMemberAsync(ChatId chatId, int userId, CancellationToken cancellationToken = default);

        Task<BotCommand[]> GetMyCommandsAsync(BotCommandScope? scope = null, string? languageCode = null, CancellationToken cancellationToken = default);

        Task<StickerSet> GetStickerSetAsync(string name, CancellationToken cancellationToken = default);

        Task<UserProfilePhotos> GetUserProfilePhotosAsync(int userId,
            int offset = 0,
            int limit = 0,
            CancellationToken cancellationToken = default);
    }
}
