using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Hosting.Options;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace Insight.TelegramBot.Hosting.WebHook;

public sealed class TelegramBotWebHookHost : IHostedService
{
    private readonly ITelegramBotClient _client;

    private readonly TelegramBotOptions _options;

    public TelegramBotWebHookHost(ITelegramBotClient client, TelegramBotOptions options)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_options.WebHook == null)
        {
            throw new ArgumentNullException(nameof(TelegramBotOptions.WebHook), "Webhook is not configured");
        }
        
        var webHookInfo = await _client.GetWebhookInfoAsync(cancellationToken);
        if (_options.WebHook.UseWebHook &&
            webHookInfo.Url.Equals(_options.WebHook.WebHookUrl,
                StringComparison.OrdinalIgnoreCase))
            return;

        await _client.DeleteWebhookAsync(_options.WebHook.DropPendingUpdatesOnDeleteWebhook,
            cancellationToken);

        if (!_options.WebHook.UseWebHook)
            throw new InvalidOperationException("You can't use this host for polling bots");

        if (string.IsNullOrWhiteSpace(_options.WebHook.WebHookUrl))
            throw new ArgumentNullException(
                $"Empty webhook url: {nameof(_options.WebHook.WebHookUrl)}");

        await _client.SetWebhookAsync(_options.WebHook.WebHookUrl,
            cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _client.DeleteWebhookAsync(_options.WebHook!.DropPendingUpdatesOnDeleteWebhook,
            cancellationToken);
    }
}