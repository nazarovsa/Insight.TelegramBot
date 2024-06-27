using System;
using System.Linq;
using Insight.TelegramBot.UpdateProcessors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Telegram.Bot;

namespace Insight.TelegramBot.DependencyInjection.Builders;

/// <summary>
/// Builder for Telegram Bot.
/// </summary>
public sealed class TelegramBotBuilder
{
    internal IServiceCollection Services { get; }

    internal bool PollingConfigured { get; set; }

    internal bool WebHookConfigured { get; set; }
    
    internal bool UpdateProcessorConfigured { get; set; }

    private TelegramBotClientBuilder? _telegramBotClientBuilder;

    private TelegramBotOptionsBuilder? _telegramBotOptionsBuilder;


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
        if (UpdateProcessorConfigured)
        {
            var registration = Services.First();
            throw new InvalidOperationException(
                $"Update handler of type {registration.ImplementationType!.Name} already registered");
        }

        var descriptor = new ServiceDescriptor(typeof(IUpdateProcessor), typeof(TUpdateProcessor), serviceLifetime);
        Services.TryAdd(descriptor);
        UpdateProcessorConfigured = true;
        return this;
    }

    public TelegramBotBuilder WithUpdateProcessor<TUpdateProcessor>(
        Func<IServiceProvider, TUpdateProcessor> updateProcessorFactory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TUpdateProcessor : IUpdateProcessor
    {
        if (UpdateProcessorConfigured)
        {
            var registration = Services.First(x => x.ServiceType == typeof(TUpdateProcessor));
            throw new InvalidOperationException($"Update processor of type {registration.ImplementationType!.Name} already registered");
        }

        var descriptor =
            new ServiceDescriptor(typeof(IUpdateProcessor), ctx => updateProcessorFactory(ctx), serviceLifetime);
        Services.TryAdd(descriptor);
        UpdateProcessorConfigured = true;
        return this;
    }
}