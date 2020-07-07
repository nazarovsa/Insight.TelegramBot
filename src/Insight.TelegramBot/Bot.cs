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

        public virtual async Task<Message> SendMessage(BotMessage message, CancellationToken cancellationToken = default)
        {
            return await Client.SendTextMessageAsync(message.ChatId, message.Text, message.ParseMode,
                message.DisableWebPagePreview, message.DisableNotification, message.ReplyToMessageId,
                message.ReplyMarkup, cancellationToken);
        }

        public virtual async Task DeleteMessage(long chatId, int messageId, CancellationToken cancellationToken = default)
        {
            await Client.DeleteMessageAsync(chatId, messageId, cancellationToken);
        }
    }
}