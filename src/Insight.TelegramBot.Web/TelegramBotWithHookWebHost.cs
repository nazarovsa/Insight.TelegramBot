using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace Insight.TelegramBot.Web
{
    public class TelegramBotWebHookHost : IHostedService
    {
        private readonly ITelegramBotClient _client;

        private readonly BotConfiguration _configuration;

        public TelegramBotWebHookHost(ITelegramBotClient client, BotConfiguration configuration)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _client = client;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var webHookInfo = await _client.GetWebhookInfoAsync(cancellationToken);
            if (webHookInfo != null)
                await _client.DeleteWebhookAsync(cancellationToken);

            if (!_configuration.UseWebHook)
                throw new InvalidOperationException("You can't use this host for polling bots");

            if (string.IsNullOrWhiteSpace(_configuration.WebHookUrl))
                throw new ArgumentNullException($"Empty webhook url: {nameof(_configuration.WebHookUrl)}");

            await _client.SetWebhookAsync(_configuration.WebHookUrl, cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.DeleteWebhookAsync(cancellationToken);
        }
    }
}