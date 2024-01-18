using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Hosting.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Insight.TelegramBot.Hosting.WebHook;

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
            var options = scope.ServiceProvider.GetRequiredService<IOptions<TelegramBotOptions>>().Value;
            if (options.WebHook == null)
            {
                throw new ArgumentNullException(nameof(TelegramBotOptions.WebHook), "Webhook is not configured");
            }
            
            try
            {
                var client = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
                var webHookInfo = await client.GetWebhookInfoAsync(cancellationToken);
                if (options.WebHook.UseWebHook &&
                    webHookInfo.Url.Equals(options.WebHook.WebHookUrl,
                        StringComparison.OrdinalIgnoreCase))
                    return;

                await client.DeleteWebhookAsync(options.WebHook.DropPendingUpdatesOnDeleteWebhook,
                    cancellationToken);

                if (!options.WebHook.UseWebHook)
                    throw new InvalidOperationException("You can't use this host for polling bots");

                if (string.IsNullOrWhiteSpace(options.WebHook.WebHookUrl))
                    throw new ArgumentNullException(
                        $"Empty webhook url: {nameof(options.WebHook.WebHookUrl)}");

                await client.SetWebhookAsync(options.WebHook.WebHookUrl,
                    cancellationToken: cancellationToken);
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
            var options = scope.ServiceProvider.GetRequiredService<IOptions<TelegramBotOptions>>().Value;
            if (options.WebHook == null)
            {
                throw new ArgumentNullException(nameof(TelegramBotOptions.WebHook), "Webhook is not configured");
            }

            var client = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            await client.DeleteWebhookAsync(options.WebHook!.DropPendingUpdatesOnDeleteWebhook,
                cancellationToken);
        }
    }
}