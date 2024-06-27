using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Insight.TelegramBot.WebHook;

public sealed class TelegramBotWebHookHost : IHostedService
{
    private readonly ILogger<TelegramBotWebHookHost> _logger;
    private readonly IServiceProvider _serviceProvider;

    public TelegramBotWebHookHost(ILogger<TelegramBotWebHookHost> logger, IServiceProvider serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var options = scope.ServiceProvider.GetRequiredService<IOptions<WebHookOptions>>().Value;
            
            try
            {
                var client = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
                var webHookInfo = await client.GetWebhookInfoAsync(cancellationToken);
                if (webHookInfo.Url.Equals(options.WebHookUrl, StringComparison.OrdinalIgnoreCase))
                    return;

                await client.DeleteWebhookAsync(options.DropPendingUpdatesOnDeleteWebhook,
                    cancellationToken);

                if (string.IsNullOrWhiteSpace(options.WebHookUrl))
                    throw new ArgumentNullException($"Empty webhook url: {nameof(options.WebHookUrl)}");

                await client.SetWebhookAsync(options.WebHookUrl, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while registering webhook");
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var options = scope.ServiceProvider.GetRequiredService<IOptions<WebHookOptions>>().Value;

            var client = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            await client.DeleteWebhookAsync(options.DropPendingUpdatesOnDeleteWebhook, cancellationToken);
        }
    }
}