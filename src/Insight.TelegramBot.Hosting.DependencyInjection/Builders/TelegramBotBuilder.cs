using System;
using System.Linq;
using Insight.TelegramBot.Hosting.Options;
using Insight.TelegramBot.UpdateProcessors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Telegram.Bot;

namespace Insight.TelegramBot.Hosting.DependencyInjection.Builders;

/// <summary>
/// Builder for Telegram Bot.
/// </summary>
public sealed class TelegramBotBuilder
{
    public IServiceCollection Services { get; }

    private PollingHostBuilder? _pollingHostBuilder;

    private WebHookHostBuilder? _webHookHostBuilder;

    private TelegramBotClientBuilder? _telegramBotClientBuilder;

    private TelegramBotOptionsBuilder? _telegramBotOptionsBuilder;

    private bool _handlingConfigured;

    public TelegramBotBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public TelegramBotBuilder WithBot<TBot>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TBot : class, IBot
    {
        Services.TryAdd(new ServiceDescriptor(typeof(IBot), typeof(TBot), serviceLifetime));
        return this;
    }

    /// <summary>
    /// Configure <see cref="TelegramBotClient"/>.
    /// </summary>
    /// <param name="builder"><see cref="_telegramBotClientBuilder"/>.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"><see cref="TelegramBotClient"/> already configured.</exception>
    public TelegramBotBuilder WithTelegramBotClient(Action<TelegramBotClientBuilder> builder)
    {
        if (_telegramBotClientBuilder != null)
        {
            throw new InvalidOperationException("Telegram bot client already configured");
        }

        _telegramBotClientBuilder = new TelegramBotClientBuilder(Services);

        builder(_telegramBotClientBuilder);
        _telegramBotClientBuilder.Build();
        return this;
    }

    public TelegramBotBuilder WithOptions(Action<TelegramBotOptionsBuilder> builder)
    {
        if (_telegramBotOptionsBuilder != null)
        {
            throw new InvalidOperationException($"{nameof(TelegramBotOptions)} already configured");
        }

        _telegramBotOptionsBuilder = new TelegramBotOptionsBuilder(Services);

        builder(_telegramBotOptionsBuilder);
        _telegramBotOptionsBuilder.Build();
        return this;
    }

    public TelegramBotBuilder WithUpdateProcessor<TUpdateProcessor>(
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TUpdateProcessor : IUpdateProcessor
    {
        if (_handlingConfigured)
        {
            var registration = Services.First();
            throw new InvalidOperationException(
                $"Update handler of type {registration.ImplementationType!.Name} already registered");
        }

        var descriptor = new ServiceDescriptor(typeof(IUpdateProcessor), typeof(TUpdateProcessor), serviceLifetime);
        Services.TryAdd(descriptor);
        _handlingConfigured = true;
        return this;
    }

    public TelegramBotBuilder WithUpdateProcessor<TUpdateProcessor>(
        Func<IServiceProvider, TUpdateProcessor> updateProcessorFactory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TUpdateProcessor : IUpdateProcessor
    {
        if (_handlingConfigured)
        {
            var registration = Services.First();
            throw new InvalidOperationException(
                $"Update handler of type {registration.ImplementationType!.Name} already registered");
        }

        var descriptor = new ServiceDescriptor(typeof(IUpdateProcessor), ctx => updateProcessorFactory(ctx), serviceLifetime);
        Services.TryAdd(descriptor);
        _handlingConfigured = true;
        return this;
    }

    public TelegramBotBuilder WithPolling(Action<PollingHostBuilder>? builder)
    {
        if (_webHookHostBuilder != null || _pollingHostBuilder != null)
        {
            throw new InvalidOperationException("Failed to add polling: builder already configured for hosted bot");
        }

        _pollingHostBuilder = new PollingHostBuilder(Services);

        builder?.Invoke(_pollingHostBuilder);
        _pollingHostBuilder.Build();
        return this;
    }

    public TelegramBotBuilder WithWebHook(Action<WebHookHostBuilder>? builder)
    {
        if (_webHookHostBuilder != null || _pollingHostBuilder != null)
        {
            throw new InvalidOperationException("Failed to add webhook: builder already configured for hosted bot");
        }

        _webHookHostBuilder = new WebHookHostBuilder(Services);
        builder?.Invoke(_webHookHostBuilder);
        _webHookHostBuilder.Build();
        return this;
    }
}