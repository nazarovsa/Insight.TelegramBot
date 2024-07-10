using System;
using Insight.TelegramBot.DependencyInjection.Builders.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.TelegramBot.DependencyInjection.Builders;

public sealed class TelegramBotOptionsBuilder : OptionsBuilderBase<TelegramBotOptions>
{
    public TelegramBotOptionsBuilder(IServiceCollection services) : base(services)
    {
    }

    public override OptionsBuilderBase<TelegramBotOptions> FromValue(TelegramBotOptions options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }
        
        Services.Configure<TelegramBotOptions>(opt =>
        {
            opt.Token = options.Token;
        });
        
        OptionsConfigured = true;
        return this;
    }
}