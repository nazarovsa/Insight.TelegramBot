using System.Reflection;
using Insight.TelegramBot.DependencyInjection.Builders;
using Insight.TelegramBot.Handling.Infrastructure;

namespace Insight.TelegramBot.DependencyInjection.Handling;

public static class TelegramBotBuilderExtensions
{
    public static TelegramBotBuilder WithHandling(this TelegramBotBuilder telegramBotBuilder, params Assembly[] assemblies)
    {
        if (telegramBotBuilder.UpdateProcessorConfigured)
        {
            throw new InvalidOperationException("Failed to add handling: builder already configured with update processor");
        }

        telegramBotBuilder.Services.AddTelegramBotHandling(assemblies);
        telegramBotBuilder.UpdateProcessorConfigured = true;
        return telegramBotBuilder;
    }
}