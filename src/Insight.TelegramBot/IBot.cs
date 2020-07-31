using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Models;
using Telegram.Bot.Types;

namespace Insight.TelegramBot
{
    public interface IBot
    {
        Task<Message> SendMessage(BotMessage message, CancellationToken cancellationToken = default);

        Task<Message> ForwardMessage(ChatId receiverId, ChatId chatId, int messageId, bool disableNotification = false,
            CancellationToken cancellationToken = default);

        Task DeleteMessage(long chatId, int messageId, CancellationToken cancellationToken = default);

        Task<Chat> GetChat(ChatId id, CancellationToken cancellationToken = default);

        Task<int> GetChatMembersCountAsync(ChatId id, CancellationToken cancellationToken = default);

        Task<User> GetMe(CancellationToken cancellationToken = default);

        Task<ChatMember[]> GetChatAdministratorsAsync(ChatId id, CancellationToken cancellationToken = default);
    }
}