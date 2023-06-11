using System;
using System.Collections.Generic;
using System.Linq;
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

        // Process update with handlers for all updates.
        logger?.LogDebug("Queueing handlers of type {HandlerType}", typeof(IUpdateHandler).FullName);
        var allUpdateHandlers = scope.ServiceProvider.GetServices<IUpdateHandler>();
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
            try
            {
                if (!await kv.Value.MatchesAsync(update, cancellationToken))
                {
                    continue;
                }
            }
            catch (Exception ex)
            {
                var logLevel = _options.ThrowBuildFlowExceptions
                    ? LogLevel.Error
                    : LogLevel.Warning;

                logger?.Log(logLevel, ex, "Exception occured while matching message with mather {MatcherType}",
                    kv.Value.GetType().FullName);

                if (_options.ThrowBuildFlowExceptions)
                {
                    throw;
                }
                
                continue;
            }

            var matchingHandlers = scope.ServiceProvider.GetServices(kv.Key);
            foreach (var matchingHandler in matchingHandlers)
            {
                if (matchingHandler is not IUpdateHandler handler)
                {
                    logger?.LogWarning("Failed to cast {HandlerType} to {IUpdateHandlerType}",
                        kv.Key.FullName,
                        typeof(IUpdateHandler).FullName);
                    continue;
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
            if (contextMatcher is not IContextUpdateMatcher matcher)
            {
                logger?.LogWarning("Failed to cast {MatcherType} to {IContextUpdateMatcherType}",
                    matcherType.FullName, nameof(IContextUpdateMatcher));
                continue;
            }

            try
            {
                if (!await matcher.MatchesAsync(update, cancellationToken))
                {
                    continue;
                }
            }
            catch (Exception ex)
            {
                var logLevel = _options.ThrowBuildFlowExceptions
                    ? LogLevel.Error
                    : LogLevel.Warning;

                logger?.Log(logLevel, ex, "Exception occured while matching message with mather {MatcherType}",
                    matcherType.FullName);

                if (_options.ThrowBuildFlowExceptions)
                {
                    throw;
                }
                
                continue;
            }

            var handlerType = typeof(IContextMatchingUpdateHandler<>).MakeGenericType(matcherType);
            var handlerObject = scope.ServiceProvider.GetService(handlerType);
            logger?.LogDebug("Resolved context handler {HandlerType} for matcher {MatcherType}",
                handlerType.FullName, matcherType.FullName);

            if (handlerObject is not IUpdateHandler handler)
            {
                logger?.LogWarning("Failed to cast {HandlerType} to {IUpdateHandlerType}", handlerType.FullName,
                    nameof(IUpdateHandler));
                continue;
            }

            handlersQueue.Enqueue(handler);
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
                var logLevel = _options.ThrowHandlingFlowExceptions
                    ? LogLevel.Error
                    : LogLevel.Warning;

                logger?.Log(logLevel, ex, "Exception occured while handling message: {UpdateHandlerType}",
                    handler.GetType().Name);

                var errorHandlers = scope.ServiceProvider.GetServices(typeof(IExceptionFlowHandler)).OfType<IExceptionFlowHandler>();
                foreach (var errorHandler in errorHandlers)
                {
                    await errorHandler.Handle(ex, CancellationToken.None);
                }
                
                if (_options.ThrowHandlingFlowExceptions)
                {
                    throw;
                }
            }
        }
    }
}