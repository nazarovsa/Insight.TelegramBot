using Insight.TelegramBot.WebHook;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.TelegramBot.DependencyInjection.WebHook;

public sealed class WebHookHostBuilder
{
    internal IServiceCollection  Services { get; }

    private WebHookOptionsBuilder? _webHookOptionsBuilder;

    public WebHookHostBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public WebHookHostBuilder WithOptions(Action<WebHookOptionsBuilder> builder)
    {
        if (_webHookOptionsBuilder != null)
        {
            throw new InvalidOperationException($"{nameof(WebHookOptionsBuilder)} already configured");
        }

        _webHookOptionsBuilder = new WebHookOptionsBuilder(Services);

        builder(_webHookOptionsBuilder);
        _webHookOptionsBuilder.Build();
        return this;
    }

    internal void Build()
    {
        Services.AddHostedService<TelegramBotWebHookHost>();
    }
}