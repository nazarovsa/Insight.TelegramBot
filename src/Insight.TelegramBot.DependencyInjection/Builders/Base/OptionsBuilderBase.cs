using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.TelegramBot.DependencyInjection.Builders.Base;

public abstract class OptionsBuilderBase<TOptions> where TOptions : class, new()
{
    protected readonly IServiceCollection Services;

    protected bool OptionsConfigured { get; set; }

    protected OptionsBuilderBase(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public OptionsBuilderBase<TOptions> FromConfiguration(IConfiguration configuration,
        string sectionName = nameof(TOptions))
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        Services.Configure<TOptions>(configuration.GetSection(sectionName));
        OptionsConfigured = true;
        return this;
    }

    public abstract OptionsBuilderBase<TOptions> FromValue(TOptions options);

    internal void Build()
    {
        if (!OptionsConfigured)
        {
            throw new InvalidOperationException($"{nameof(TOptions)} is not configured");
        }
    }
}