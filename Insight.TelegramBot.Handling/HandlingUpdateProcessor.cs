using System;
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
        foreach (var kv in _updateHandlersProvider.TypeMap)
        {
            if (kv.Value.Matches(update))
            {
                using var scope = _serviceProvider.CreateScope();
                var services = scope.ServiceProvider.GetServices(kv.Key);
                foreach (var service in services)
                {
                    if (service is IUpdateHandler updateHandler)
                    {
                        await updateHandler.Handle(update, cancellationToken);
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
}