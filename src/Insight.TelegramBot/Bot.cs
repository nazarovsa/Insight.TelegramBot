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

        public virtual Task<Message> SendMessage(BotMessage message,
            CancellationToken cancellationToken = default)
        {
            return Client.SendTextMessageAsync(message.ChatId, message.Text, message.ParseMode,
                message.DisableWebPagePreview, message.DisableNotification, message.ReplyToMessageId,
                message.ReplyMarkup, cancellationToken);
        }

        public virtual Task<Chat> GetChat(ChatId id, CancellationToken cancellationToken = default)
        {
            return Client.GetChatAsync(id, cancellationToken);
        }

        public virtual Task<int> GetChatMembersCountAsync(ChatId id, CancellationToken cancellationToken = default)
        {
            return Client.GetChatMembersCountAsync(id, cancellationToken);
        }

        public virtual Task<User> GetMe(CancellationToken cancellationToken = default)
        {
            return Client.GetMeAsync(cancellationToken);
        }

        public virtual Task<ChatMember[]> GetChatAdministratorsAsync(ChatId id,
            CancellationToken cancellationToken = default)
        {
            return Client.GetChatAdministratorsAsync(id, cancellationToken);
        }

        public virtual Task DeleteMessage(long chatId, int messageId,
            CancellationToken cancellationToken = default)
        {
            return Client.DeleteMessageAsync(chatId, messageId, cancellationToken);
        }
    }
}