using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Hosting.Options;
using Insight.TelegramBot.Hosting.Polling.ExceptionHandlers;
using Insight.TelegramBot.UpdateProcessors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Insight.TelegramBot.Hosting.Polling;

public sealed class TelegramBotPollingWebHost : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ILogger<TelegramBotPollingWebHost> _logger;

    private readonly ITelegramBotClient _client;

    private readonly TelegramBotOptions _telegramBotOptions;

    private Task? _pollingTask;

    public TelegramBotPollingWebHost(IServiceProvider serviceProvider,
        ILogger<TelegramBotPollingWebHost> logger,
        IOptions<TelegramBotOptions> telegramBotOptions)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _telegramBotOptions = telegramBotOptions.Value ?? throw new ArgumentNullException(nameof(telegramBotOptions));
        _client = serviceProvider.GetRequiredService<ITelegramBotClient>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var receiverOptions = _telegramBotOptions.Polling.ReceiverOptions ?? new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_pollingTask == null)
                {
                    _pollingTask = _client.ReceiveAsync(HandleUpdateAsync, HandlePollingErrorAsync, receiverOptions,
                        stoppingToken);
                }

                if (_pollingTask.Status >= TaskStatus.RanToCompletion)
                {
                    _logger.LogWarning("Polling task failed unexpectedly. Task status was {TaskStatus}",
                        _pollingTask.Status);

                    if (_pollingTask.Exception != null)
                    {
                        _logger.LogWarning(_pollingTask.Exception, "Exception at failed polling task logged");
                    }

                    _pollingTask = _client.ReceiveAsync(HandleUpdateAsync, HandlePollingErrorAsync, receiverOptions,
                        stoppingToken);
                }

                await Task.Delay(_telegramBotOptions.Polling.PollingTaskCheckInterval, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occured while checking polling task");
                await Task.Delay(_telegramBotOptions.Polling.PollingTaskExceptionDelay, stoppingToken);
            }
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
        var exceptionHandler = scope.ServiceProvider.GetService<IPollingExceptionHandler>();
        if (exceptionHandler == null)
        {
            _logger.LogError(exception, "Exception occured during polling");
            return;
        }

        await exceptionHandler.HandlePollingException(exception, cancellationToken);
    }
}