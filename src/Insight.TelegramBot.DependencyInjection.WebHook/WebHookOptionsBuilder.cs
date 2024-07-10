using Insight.TelegramBot.DependencyInjection.Builders.Base;
using Insight.TelegramBot.WebHook;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.TelegramBot.DependencyInjection.WebHook;

public sealed class WebHookOptionsBuilder : OptionsBuilderBase<WebHookOptions>
{
    public WebHookOptionsBuilder(IServiceCollection services) : base(services)
    {
    }
    
    public WebHookOptionsBuilder FromConfiguration(IConfiguration configuration)
    {
        base.FromConfiguration(configuration, $"{nameof(TelegramBotOptions)}:{nameof(WebHookOptions)}");
        return this;
    }

    public override OptionsBuilderBase<WebHookOptions> FromValue(WebHookOptions options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        Services.Configure<WebHookOptions>(opt =>
        {
            opt.WebHookPath = options.WebHookPath;
            opt.WebHookBaseUrl = options.WebHookBaseUrl;
            opt.DropPendingUpdatesOnDeleteWebhook = options.DropPendingUpdatesOnDeleteWebhook;
        });

        OptionsConfigured = true;
        return this;
    }
}