using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Configurations;
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
            Config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public virtual async Task Start()
        {
            var webHookInfo = await Client.GetWebhookInfoAsync();
            if (webHookInfo != null)
            {
                if (Config.WebHookConfiguration.UseWebHook &&
                    webHookInfo.Url.Equals(Config.WebHookConfiguration.WebHookUrl, StringComparison.OrdinalIgnoreCase))
                    return;

                await Client.DeleteWebhookAsync();
            }

            if (Config.WebHookConfiguration.UseWebHook)
            {
                if (string.IsNullOrWhiteSpace(Config.WebHookConfiguration.WebHookUrl))
                    throw new ArgumentNullException(
                        $"Empty webhook url: {nameof(Config.WebHookConfiguration.WebHookUrl)}");

                await Client.SetWebhookAsync(Config.WebHookConfiguration.WebHookUrl);
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
            if (Config.WebHookConfiguration.UseWebHook)
                await Client.DeleteWebhookAsync();
            else
                Client.StopReceiving();
        }

        public abstract Task ProcessMessage(Message message, CancellationToken cancellationToken = default);

        public abstract Task ProcessCallback(CallbackQuery callbackQuery, CancellationToken cancellationToken = default);
    }
}