using System;
using System.Net.Http;
using Insight.TelegramBot.Hosting.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Insight.TelegramBot.Hosting.DependencyInjection.Builders;

public sealed class TelegramBotClientBuilder
{
    private readonly IServiceCollection _services;

    private ServiceLifetime _serviceLifetime = ServiceLifetime.Singleton;

    private Func<IServiceProvider, HttpClient>? _httpClientFactory;

    public TelegramBotClientBuilder(IServiceCollection services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public TelegramBotClientBuilder WithMicrosoftHttpClientFactory(Action<HttpClient>? configureHttpClient = null)
    {
        if (_httpClientFactory != null)
        {
            throw new InvalidOperationException("Http client factory already configured");
        }

        _services.AddHttpClient(nameof(TelegramBotClient), client => configureHttpClient?.Invoke(client));

        _httpClientFactory = ctx => ctx.GetRequiredService<IHttpClientFactory>()
            .CreateClient(nameof(TelegramBotClient));

        return this;
    }

    public TelegramBotClientBuilder WithHttpClientFactory(Func<IServiceProvider, HttpClient> factory)
    {
        if (_httpClientFactory != null)
        {
            throw new InvalidOperationException("Http client factory already configured");
        }

        _httpClientFactory = factory;

        return this;
    }

    public TelegramBotClientBuilder WithLifetime(ServiceLifetime serviceLifetime)
    {
        _serviceLifetime = serviceLifetime;
        return this;
    }

    internal void Build()
    {
        ServiceDescriptor? descriptor;
        if (_httpClientFactory == null)
        {
            descriptor = new ServiceDescriptor(typeof(ITelegramBotClient),
                ctx => new TelegramBotClient(ctx.GetRequiredService<IOptions<TelegramBotOptions>>().Value.Token),
                _serviceLifetime);
        }
        else
        {
            descriptor = new ServiceDescriptor(typeof(ITelegramBotClient),
                ctx => new TelegramBotClient(ctx.GetRequiredService<IOptions<TelegramBotOptions>>().Value.Token,
                    _httpClientFactory.Invoke(ctx)),
                _serviceLifetime);
        }

        _services.TryAdd(descriptor);
    }
}