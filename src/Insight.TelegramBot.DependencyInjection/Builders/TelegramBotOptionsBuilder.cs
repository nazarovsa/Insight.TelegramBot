using System;
using Insight.TelegramBot.DependencyInjection.Builders.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Insight.TelegramBot.DependencyInjection.Builders;

public sealed class TelegramBotOptionsBuilder : OptionsBuilderBase<TelegramBotClientOptions>
{
    public TelegramBotOptionsBuilder(IServiceCollection services) : base(services)
    {
    }
}