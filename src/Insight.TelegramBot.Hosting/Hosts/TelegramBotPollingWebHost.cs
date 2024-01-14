using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Configurations;
using Insight.TelegramBot.UpdateProcessors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Insight.TelegramBot.Hosting.Hosts;

internal sealed class TelegramBotPollingWebHost : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ILogger<TelegramBotPollingWebHost> _logger;

    private readonly ITelegramBotClient _client;

    private readonly BotConfiguration _botConfiguration;

    private readonly ReceiverOptions? _receiverOptions;

    private Task? _pollingTask;

    public TelegramBotPollingWebHost(IServiceProvider serviceProvider,
        ILogger<TelegramBotPollingWebHost> logger,
        IOptions<BotConfiguration> config,
        ITelegramBotClient client,
        ReceiverOptions? receiverOptions = null)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _botConfiguration = config.Value ?? throw new ArgumentNullException(nameof(config));
        _receiverOptions = receiverOptions;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_botConfiguration.WebHookConfiguration is { UseWebHook: true })
            throw new InvalidOperationException("For webhook bots use TelegramBotWithHookWebHost");

        var receiverOptions = _receiverOptions ?? new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            if (_pollingTask == null)
            {
                _pollingTask = _client.ReceiveAsync(HandleUpdateAsync, HandlePollingErrorAsync, receiverOptions,
                    stoppingToken);
            }

            if (_pollingTask.Status >= TaskStatus.RanToCompletion)
            {
                _logger.LogWarning(
                    "Polling task failed unexpectedly. Task status was {TaskStatus}",
                    _pollingTask.Status);
                _pollingTask = _client.ReceiveAsync(HandleUpdateAsync, HandlePollingErrorAsync, receiverOptions,
                    stoppingToken);
            }

            await Task.Delay(_botConfiguration.PollingTaskCheckInterval, stoppingToken);
        }
    }

    private async Task HandleUpdateAsync(ITelegramBotClient client, Update update,
        CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var updateProcessor = scope.ServiceProvider.GetRequiredService<IUpdateProcessor>();
        await updateProcessor.HandleUpdate(update, cancellationToken);
    }

    private async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception,
        CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var updateProcessor = scope.ServiceProvider.GetRequiredService<IPollingExceptionHandler>();
        await updateProcessor.HandlePollingErrorAsync(exception, cancellationToken);
    }
}