using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Configurations;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace Insight.TelegramBot.Web.Hosts
{
    public class TelegramBotWebHookHost : IHostedService
    {
        private readonly ITelegramBotClient _client;

        private readonly BotConfiguration _configuration;

        public TelegramBotWebHookHost(ITelegramBotClient client, BotConfiguration configuration)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var webHookInfo = await _client.GetWebhookInfoAsync(cancellationToken);
            if (webHookInfo != null)
            {
                if (_configuration.WebHookConfiguration.UseWebHook &&
                    webHookInfo.Url.Equals(_configuration.WebHookConfiguration.WebHookUrl, StringComparison.OrdinalIgnoreCase))
                    return;

                await _client.DeleteWebhookAsync(cancellationToken);
            }
            
            if (!_configuration.WebHookConfiguration.UseWebHook)
                throw new InvalidOperationException("You can't use this host for polling bots");

            if (string.IsNullOrWhiteSpace(_configuration.WebHookConfiguration.WebHookUrl))
                throw new ArgumentNullException($"Empty webhook url: {nameof(_configuration.WebHookConfiguration.WebHookUrl)}");

            await _client.SetWebhookAsync(_configuration.WebHookConfiguration.WebHookUrl, cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.DeleteWebhookAsync(cancellationToken);
        }
    }
}