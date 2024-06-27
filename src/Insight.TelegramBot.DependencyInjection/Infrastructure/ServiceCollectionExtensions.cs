using System;
using Insight.TelegramBot.DependencyInjection.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.TelegramBot.DependencyInjection.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services,
        Action<TelegramBotBuilder> builderConfiguration)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var builder = new TelegramBotBuilder(services);
        builderConfiguration(builder);
        return services;
    }
}