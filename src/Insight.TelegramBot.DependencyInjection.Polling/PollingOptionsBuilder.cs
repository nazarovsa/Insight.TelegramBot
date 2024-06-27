using System;
using Insight.TelegramBot.DependencyInjection.Builders.Base;
using Insight.TelegramBot.Polling;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.TelegramBot.DependencyInjection.Polling;

public sealed class PollingOptionsBuilder : OptionsBuilderBase<PollingOptions>
{
    public PollingOptionsBuilder(IServiceCollection services) : base(services)
    {
    }

    public override OptionsBuilderBase<PollingOptions> FromValue(PollingOptions options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        Services.Configure<PollingOptions>(opt =>
        {
            opt.ReceiverOptions = options.ReceiverOptions;
            opt.PollingTaskCheckInterval = options.PollingTaskCheckInterval;
            opt.PollingTaskExceptionDelay = options.PollingTaskExceptionDelay;
        });

        OptionsConfigured = true;
        return this;
    }
}