using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Handling.Handlers;
using Insight.TelegramBot.Handling.Matchers;
using Insight.TelegramBot.UpdateProcessors;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling;

internal sealed class HandlingUpdateProcessor : IUpdateProcessor
{
    private readonly IServiceProvider _serviceProvider;

    private static readonly Dictionary<Type, IUpdateMatcher> _typeMatchersMap = new();

    public HandlingUpdateProcessor(IServiceProvider serviceProvider, IUpdateHandlersProvider updateHandlersProvider)
    {
        _serviceProvider = serviceProvider;

        foreach (var pair in updateHandlersProvider.GetTypeMap())
        {
            if (!_typeMatchersMap.ContainsKey(pair.Key))
            {
                _typeMatchersMap.Add(pair.Key, pair.Value);
            }
        }
    }

    /// <summary>
    /// Executes all handlers which satisfies matcher condition from matchers map. 
    /// </summary>
    /// <param name="update"><see cref="Update"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    public async Task HandleUpdate(Update update, CancellationToken cancellationToken = default)
    {
        foreach (var kv in _typeMatchersMap)
        {
            if (kv.Value.Matches(update))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService(kv.Key) as IUpdateHandler;
                    if (service == null)
                    {
                        throw new InvalidOperationException(
                            $"There is no registered implementation for {kv.Key.Name}.");
                    }

                    await service.Handle(update, cancellationToken);
                }
            }
        }
    }
}