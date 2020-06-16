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
        private readonly BotConfiguration _config;

        private readonly ITelegramBotClient _client;

        protected Bot(BotConfiguration config,
            ITelegramBotClient client)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _config = config;
            _client = client;
        }


        public virtual async Task Start()
        {
            await _client.DeleteWebhookAsync();
            if (_config.UseWebHook)
            {
                if (string.IsNullOrWhiteSpace(_config.WebHookUrl))
                    throw new ArgumentNullException($"Empty webhook url: {nameof(_config.WebHookUrl)}");

                await _client.SetWebhookAsync(_config.WebHookUrl);
            }
            else
            {
                _client.OnMessage += async (s, e) => await ProcessMessage(e.Message);
                _client.OnCallbackQuery += async (s, e) => await ProcessCallback(e.CallbackQuery);
                _client.StartReceiving();
            }
        }

        public virtual async Task Stop()
        {
            if (_config.UseWebHook)
                await _client.DeleteWebhookAsync();
            else
                _client.StopReceiving();
        }

        public virtual async Task<Message> SendMessage(BotMessage message, CancellationToken token = default)
        {
            return await _client.SendTextMessageAsync(message.ChatId, message.Text, message.ParseMode,
                message.DisableWebPagePreview, message.DisableNotification, message.ReplyToMessageId,
                message.ReplyMarkup, token);
        }

        public virtual async Task DeleteMessage(long chatId, int messageId, CancellationToken token = default)
        {
            await _client.DeleteMessageAsync(chatId, messageId, token);
        }

        public abstract Task ProcessMessage(Message message, CancellationToken token = default);

        public abstract Task ProcessCallback(CallbackQuery query, CancellationToken token = default);
    }
}