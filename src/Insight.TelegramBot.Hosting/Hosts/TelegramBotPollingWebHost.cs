using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Configurations;
using Insight.TelegramBot.UpdateProcessors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Insight.TelegramBot.Hosting.Hosts;

internal sealed class TelegramBotPollingWebHost : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ITelegramBotClient _client;

    private readonly BotConfiguration _botConfiguration;

    private readonly ReceiverOptions? _receiverOptions;

    private CancellationTokenSource? _cts;

    public TelegramBotPollingWebHost(IServiceProvider serviceProvider,
        IOptions<BotConfiguration> config,
        ITelegramBotClient client,
        ReceiverOptions? receiverOptions = null)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _botConfiguration = config.Value ?? throw new ArgumentNullException(nameof(config));
        _receiverOptions = receiverOptions;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_botConfiguration.WebHookConfiguration is { UseWebHook: true })
            throw new InvalidOperationException("For webhook bots use TelegramBotWithHookWebHost");

        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        var receiverOptions = _receiverOptions ?? new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        await _client.ReceiveAsync(HandleUpdateAsync, HandlePollingErrorAsync, receiverOptions, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        _cts?.Cancel();
        return Task.CompletedTask;
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