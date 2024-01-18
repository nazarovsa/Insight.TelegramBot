using System;
using Insight.TelegramBot.Hosting.Options;
using Insight.TelegramBot.Hosting.Polling;
using Insight.TelegramBot.Hosting.Polling.ExceptionHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Insight.TelegramBot.Hosting.DependencyInjection.Builders;

public sealed class PollingHostBuilder
{
    private readonly IServiceCollection _services;

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

    internal void Build()
    {
        // TODO: rewrite ctor to accept IServiceProvider
        _services.AddHostedService(ctx =>
            new TelegramBotPollingWebHost(ctx.GetRequiredService<IServiceProvider>(),
                ctx.GetRequiredService<ILogger<TelegramBotPollingWebHost>>(),
                ctx.GetRequiredService<IOptions<TelegramBotOptions>>(),
                ctx.GetRequiredService<ITelegramBotClient>(),
                ctx.GetRequiredService<ReceiverOptions>()));

        _services.TryAddSingleton<IPollingExceptionHandler, NulloPollingExceptionHandler>();
    }
}