using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Handling.Handlers;
using Insight.TelegramBot.UpdateProcessors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling;

internal sealed class HandlingUpdateProcessor : IUpdateProcessor
{
    private readonly ILogger<HandlingUpdateProcessor> _logger;
    private readonly HandlingUpdateProcessorOptions _options;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUpdateHandlersProvider _updateHandlersProvider;

    public HandlingUpdateProcessor(ILogger<HandlingUpdateProcessor> logger,
        IServiceProvider serviceProvider,
        IOptionsSnapshot<HandlingUpdateProcessorOptions> options,
        IUpdateHandlersProvider updateHandlersProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _options = options?.Value ?? new HandlingUpdateProcessorOptions();
        _updateHandlersProvider = updateHandlersProvider;
    }

    /// <summary>
    /// Executes all handlers which satisfies matcher condition from matchers map. 
    /// </summary>
    /// <param name="update"><see cref="Update"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    public async Task HandleUpdate(Update update, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var allUpdateHandlers = scope.ServiceProvider
            .GetServices<IUpdateHandler>();

        // Process update with handlers for all updates.
        foreach (var updateHandler in allUpdateHandlers)
        {
            await HandleWithExceptionHandling(updateHandler, update, cancellationToken);
        }

        foreach (var kv in _updateHandlersProvider.TypeMap)
        {
            if (!kv.Value.Matches(update))
                continue;

            var matchingHandlers = scope.ServiceProvider.GetServices(kv.Key);
            foreach (var service in matchingHandlers)
            {
                if (service is IUpdateHandler updateHandler)
                {
                    await HandleWithExceptionHandling(updateHandler, update, cancellationToken);
                }
                else
                {
                    _logger.LogWarning("Update handler resolved for {ResolvedType} is not a {TypeName}",
                        kv.Key.Name,
                        nameof(IUpdateHandler));
                }
            }
        }
    }

    private async Task HandleWithExceptionHandling(IUpdateHandler handler,
        Update update,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await handler.Handle(update, cancellationToken);
        }
        catch (Exception ex)
        {
            var logLevel = _options.ThrowFlowExceptions
                ? LogLevel.Error
                : LogLevel.Warning;

            _logger.Log(logLevel, ex, "Exception occured while handling message: {UpdateHandlerType}",
                handler.GetType().Name);

            if (_options.ThrowFlowExceptions)
            {
                throw;
            }
        }
    }
}
