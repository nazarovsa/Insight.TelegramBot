using System;
using Insight.TelegramBot.Hosting.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.TelegramBot.Hosting.Infrastructure;

public sealed class TelegramBotOptionsBuilder
{
    private readonly IServiceCollection _services;

    private bool _optionsConfigured;

    public TelegramBotOptionsBuilder(IServiceCollection services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public TelegramBotOptionsBuilder FromConfiguration(IConfiguration configuration,
        string sectionName = nameof(TelegramBotOptions))
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        _services.Configure<TelegramBotOptions>(configuration.GetSection(sectionName));
        _optionsConfigured = true;
        return this;
    }

    public TelegramBotOptionsBuilder FromValue(TelegramBotOptions options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        _services.Configure<TelegramBotOptions>(opt =>
        {
            opt.Token = options.Token;
            opt.WebHook = options.WebHook;
            opt.Polling = options.Polling;
        });

        _optionsConfigured = true;
        return this;
    }

    public void Build()
    {
        if (!_optionsConfigured)
        {
            throw new InvalidOperationException($"{nameof(TelegramBotOptions)} is not configured");
        }
    }
}