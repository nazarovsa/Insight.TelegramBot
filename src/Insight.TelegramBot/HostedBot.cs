using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Insight.TelegramBot
{
    public abstract class HostedBot : Bot, IHostedBot
    {
        protected BotConfiguration Config { get; }

        protected HostedBot(BotConfiguration config, ITelegramBotClient client)
            : base(client)
        {
            if (Config == null)
                throw new ArgumentNullException(nameof(config));
            
            Config = config;
        }

        public virtual async Task Start()
        {
            var webHookInfo = await Client.GetWebhookInfoAsync();
            if (webHookInfo != null && !Config.UseWebHook)
                await Client.DeleteWebhookAsync();

            if (Config.UseWebHook)
            {
                if (string.IsNullOrWhiteSpace(Config.WebHookUrl))
                    throw new ArgumentNullException($"Empty webhook url: {nameof(Config.WebHookUrl)}");

                await Client.SetWebhookAsync(Config.WebHookUrl);
            }
            else
            {
                Client.OnMessage += async (s, e) => await ProcessMessage(e.Message);
                Client.OnCallbackQuery += async (s, e) => await ProcessCallback(e.CallbackQuery);
                Client.StartReceiving();
            }
        }

        public virtual async Task Stop()
        {
            if (Config.UseWebHook)
                await Client.DeleteWebhookAsync();
            else
                Client.StopReceiving();
        }

        public abstract Task ProcessMessage(Message message, CancellationToken token = default);

        public abstract Task ProcessCallback(CallbackQuery query, CancellationToken token = default);
    }
}