using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Models;
using Telegram.Bot.Types;

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

        Task<Message> ForwardMessageAsync(ChatId receiverId, ChatId chatId, int messageId,
            bool disableNotification = false,
            CancellationToken cancellationToken = default);

        Task DeleteMessageAsync(long chatId, int messageId, CancellationToken cancellationToken = default);

        Task<Chat> GetChatAsync(ChatId id, CancellationToken cancellationToken = default);

        Task<int> GetChatMembersCountAsync(ChatId id, CancellationToken cancellationToken = default);

        Task<User> GetMeAsync(CancellationToken cancellationToken = default);

        Task<ChatMember[]> GetChatAdministratorsAsync(ChatId id, CancellationToken cancellationToken = default);

        Task<File> GetFileAsync(string id, CancellationToken cancellationToken = default);

        Task<ChatMember> GetChatMemberAsync(ChatId chatId, int userId, CancellationToken cancellationToken = default);

        Task<BotCommand[]> GetMyCommandsAsync(CancellationToken cancellationToken = default);

        Task<StickerSet> GetStickerSetAsync(string name, CancellationToken cancellationToken = default);

        Task<UserProfilePhotos> GetUserProfilePhotosAsync(int userId, int offset = 0, int limit = 0,
            CancellationToken cancellationToken = default);
    }
}