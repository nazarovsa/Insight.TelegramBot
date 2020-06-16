using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Insight.TelegramBot
{
    public abstract class Bot : IBot
    {
        protected BotConfiguration Config { get; private set; }

        protected ITelegramBotClient Client { get; private set; }

        protected Bot(BotConfiguration config,
            ITelegramBotClient client)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            if (client == null)
                throw new ArgumentNullException(nameof(client));

            Config = config;
            Client = client;
        }


        public virtual async Task<Message> SendMessage(BotMessage message, CancellationToken token = default)
        {
            return await Client.SendTextMessageAsync(message.ChatId, message.Text, message.ParseMode,
                message.DisableWebPagePreview, message.DisableNotification, message.ReplyToMessageId,
                message.ReplyMarkup, token);
        }

        public virtual async Task DeleteMessage(long chatId, int messageId, CancellationToken token = default)
        {
            await Client.DeleteMessageAsync(chatId, messageId, token);
        }
    }
}