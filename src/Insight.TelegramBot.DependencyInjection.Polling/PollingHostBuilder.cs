using System;
using Insight.TelegramBot.Polling;
using Insight.TelegramBot.Polling.ExceptionHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Insight.TelegramBot.DependencyInjection.Polling;

public sealed class PollingHostBuilder
{
    private readonly IServiceCollection _services;

    private PollingOptionsBuilder? _pollingOptionsBuilder;

    public PollingHostBuilder(IServiceCollection services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public PollingHostBuilder WithExceptionHandler<TExceptionHandler>()
        where TExceptionHandler : class, IPollingExceptionHandler
    {
        _services.AddSingleton<IPollingExceptionHandler, TExceptionHandler>();
        return this;
    }
    
    public PollingHostBuilder WithOptions(Action<PollingOptionsBuilder> builder)
    {
        if (_pollingOptionsBuilder != null)
        {
            throw new InvalidOperationException($"{nameof(PollingOptionsBuilder)} already configured");
        }

        _pollingOptionsBuilder = new PollingOptionsBuilder(_services);

        builder(_pollingOptionsBuilder);
        _pollingOptionsBuilder.Build();
        return this;
    }

    internal void Build()
    {
        _services.AddHostedService<TelegramBotPollingWebHost>();

        _services.TryAddSingleton<IPollingExceptionHandler, NulloPollingExceptionHandler>();
    }
}