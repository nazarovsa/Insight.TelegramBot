using System;
using System.Reflection;
using Insight.TelegramBot.Hosting.Options;
using Insight.TelegramBot.Hosting.WebHook;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Insight.TelegramBot.Hosting.DependencyInjection.Builders;

public sealed class WebHookHostBuilder
{
    private readonly IServiceCollection _services;

    public WebHookHostBuilder(IServiceCollection services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Registers <see cref="UpdateController"/> as a service
    /// </summary>
    /// <param name="builder"><see cref="IMvcBuilder"/></param>
    /// <returns></returns>
    public WebHookHostBuilder WithDefaultUpdateController()
    {
        var assembly = Assembly.Load(typeof(UpdateController).Assembly.GetName());
        if (assembly == null)
            throw new ArgumentNullException(nameof(assembly));

        _services.AddMvc()
            .AddApplicationPart(assembly)
            .AddControllersAsServices();

        return this;
    }

    internal void Build()
    {
        _services.AddHostedService<TelegramBotWebHookHost>();
    }
}