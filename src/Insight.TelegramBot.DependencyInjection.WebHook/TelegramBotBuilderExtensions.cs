using Insight.TelegramBot.DependencyInjection.Builders;

namespace Insight.TelegramBot.DependencyInjection.WebHook;

public static class TelegramBotBuilderExtensions
{
    public static TelegramBotBuilder WithWebHook(this TelegramBotBuilder telegramBotBuilder,
        Action<WebHookHostBuilder>? builder)
    {
        if (telegramBotBuilder.PollingConfigured)
        {
            throw new InvalidOperationException("Failed to add polling: builder already configured with polling");
        }

        if (telegramBotBuilder.WebHookConfigured)
        {
            throw new InvalidOperationException("Failed to add polling: builder already configured with webhook");
        }

        var pollingHostBuilder = new WebHookHostBuilder(telegramBotBuilder.Services);

        builder?.Invoke(pollingHostBuilder);
        pollingHostBuilder.Build();
        telegramBotBuilder.WebHookConfigured = true;
        return telegramBotBuilder;
    }
}