using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Handling.Handlers;
using Insight.TelegramBot.Handling.Matchers;
using Insight.TelegramBot.UpdateProcessors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling;

internal sealed class HandlingUpdateProcessor : IUpdateProcessor
{
    private readonly HandlingUpdateProcessorOptions _options;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUpdateHandlersProvider _updateHandlersProvider;

    public HandlingUpdateProcessor(
        IServiceProvider serviceProvider,
        IOptionsSnapshot<HandlingUpdateProcessorOptions> options,
        IUpdateHandlersProvider updateHandlersProvider)
    {
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
        var handlersQueue = new Queue<IUpdateHandler>();

        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetService<ILogger<HandlingUpdateProcessor>>();
        var allUpdateHandlers = scope.ServiceProvider
            .GetServices<IUpdateHandler>();

        // Process update with handlers for all updates.
        logger?.LogDebug("Queueing handlers of type {HandlerType}", typeof(IUpdateHandler).FullName);
        foreach (var updateHandler in allUpdateHandlers)
        {
            handlersQueue.Enqueue(updateHandler);
        }

        logger?.LogDebug("Queued handlers of type {HandlerType}. Total: {TotalQueuedHandlers}",
            typeof(IUpdateHandler).FullName, handlersQueue.Count);

        // Handle maching update handlers
        logger?.LogDebug("Queueing handlers of type {HandlerType}", typeof(IMatchingUpdateHandler<>).FullName);
        foreach (var kv in _updateHandlersProvider.TypeMap)
        {
            if (!await kv.Value.MatchesAsync(update))
                continue;

            var matchingHandlers = scope.ServiceProvider.GetServices(kv.Key);
            foreach (var matchingHandler in matchingHandlers)
            {
                var handler = matchingHandler as IUpdateHandler;
                if (handler == null)
                {
                    logger?.LogWarning("Failed to cast {HandlerType} to {IUpdateHandlerType}",
                        kv.Key.Name,
                        nameof(IUpdateHandler));
                }

                handlersQueue.Enqueue(handler);
            }
        }
        logger?.LogDebug("Queued handlers of type {HandlerType}. Total: {TotalQueuedHandlers}",
            typeof(IMatchingUpdateHandler<>).FullName, handlersQueue.Count);

        // Handle context matching update handlers 
        logger?.LogDebug("Queueing handlers of type {HandlerType}", typeof(IContextMatchingUpdateHandler<>).FullName);
        var contextMatchers = scope.ServiceProvider.GetServices(typeof(IContextUpdateMatcher));
        foreach (var contextMatcher in contextMatchers)
        {
            var matcherType = contextMatcher!.GetType();
            logger?.LogDebug("Resolved context matcher {MatcherType}", matcherType.FullName);
            var matcher = contextMatcher as IContextUpdateMatcher;
            if (matcher == null)
            {
                logger?.LogWarning("Failed to cast {MatcherType} to {IContextUpdateMatcherType}",
                    matcherType.FullName, nameof(IContextUpdateMatcher));
                continue;
            }

            if (await matcher.MatchesAsync(update))
            {
                var handlerType = typeof(IContextMatchingUpdateHandler<>).MakeGenericType(matcherType);
                var handlerObject = scope.ServiceProvider.GetService(handlerType);
                logger?.LogDebug("Resolved context handler {HandlerType} for matcher {MatcherType}",
                    handlerType.FullName, matcherType.FullName);

                var handler = handlerObject as IUpdateHandler;
                if (handler == null)
                {
                    logger?.LogWarning("Failed to cast {HandlerType} to {IUpdateHandlerType}", handlerType.FullName,
                        nameof(IUpdateHandler));
                    continue;
                }

                handlersQueue.Enqueue(handler);
            }
        }
        logger?.LogDebug("Queued handlers of type {HandlerType}. Total: {TotalQueuedHandlers}",
            typeof(IContextMatchingUpdateHandler<>).FullName, handlersQueue.Count);

        while (handlersQueue.TryDequeue(out var handler))
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

                logger?.Log(logLevel, ex, "Exception occured while handling message: {UpdateHandlerType}",
                    handler.GetType().Name);

                if (_options.ThrowFlowExceptions)
                {
                    throw;
                }
            }
        }
    }
}