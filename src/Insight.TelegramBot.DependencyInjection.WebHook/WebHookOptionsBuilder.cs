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
}