using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Handling.Handlers;
using Insight.TelegramBot.UpdateProcessors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling;

internal sealed class HandlingUpdateProcessor : IUpdateProcessor
{
    private readonly ILogger<HandlingUpdateProcessor> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUpdateHandlersProvider _updateHandlersProvider;

    private readonly bool _throwFlowExceptions = false;

    public HandlingUpdateProcessor(ILogger<HandlingUpdateProcessor> logger,
        IServiceProvider serviceProvider,
        IUpdateHandlersProvider updateHandlersProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
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
            // TODO : Extract to method HandleWithExceptions.
            try
            {
                await updateHandler.Handle(update, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Exception occured while handling message: {UpdateHandlerType}",
                    updateHandler.GetType().Name);

                if (_throwFlowExceptions)
                {
                    throw;
                }
            }
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
                    // TODO : Extract to method HandleWithExceptions.
                    try
                    {
                        await updateHandler.Handle(update, cancellationToken);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogWarning(exception, "Exception occured while handling message: {UpdateHandlerType}",
                            updateHandler.GetType().Name);
                        if (_throwFlowExceptions)
                        {
                            throw;
                        }
                    }
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
}