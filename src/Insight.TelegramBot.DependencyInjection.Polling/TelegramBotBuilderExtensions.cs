using System;
using Insight.TelegramBot.DependencyInjection.Builders;

namespace Insight.TelegramBot.DependencyInjection.Polling;

public static class TelegramBotBuilderExtensions
{
    public static TelegramBotBuilder WithPolling(this TelegramBotBuilder telegramBotBuilder,
        Action<PollingHostBuilder>? builder)
    {
        if (telegramBotBuilder.PollingConfigured)
        {
            throw new InvalidOperationException("Failed to add polling: builder already configured with polling");
        }

        if (telegramBotBuilder.WebHookConfigured)
        {
            throw new InvalidOperationException("Failed to add polling: builder already configured with webhook");
        }

        var pollingHostBuilder = new PollingHostBuilder(telegramBotBuilder.Services);

        builder?.Invoke(pollingHostBuilder);
        pollingHostBuilder.Build();
        telegramBotBuilder.PollingConfigured = true;
        return telegramBotBuilder;
    }
}