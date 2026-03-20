using System;
using Insight.TelegramBot.DependencyInjection.Builders.Base;
using Insight.TelegramBot.Polling;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.TelegramBot.DependencyInjection.Polling;

public sealed class PollingOptionsBuilder : OptionsBuilderBase<PollingOptions>
{
    public PollingOptionsBuilder(IServiceCollection services) : base(services)
    {
    }
}