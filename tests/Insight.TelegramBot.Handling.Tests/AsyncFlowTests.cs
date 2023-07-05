using Insight.TelegramBot.Handling.Handlers;
using Insight.TelegramBot.Handling.Infrastructure;
using Insight.TelegramBot.Handling.Matchers;
using Insight.TelegramBot.Handling.Tests.Handlers;
using Insight.TelegramBot.Samples.Handling.StartMessage;
using Insight.TelegramBot.UpdateProcessors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Telegram.Bot.Types;
using Xunit;

namespace Insight.TelegramBot.Handling.Tests;

public class AsyncFlowTests
{
    [Fact]
    public async Task Executes_before_and_handlers_in_the_same_async_context()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTelegramBotHandling(typeof(UserContextBeforeHandler).Assembly);
        services.Configure<HandlingUpdateProcessorOptions>(opt => { opt.ExecuteHandlersAtSameAsyncContext = true; });
        services.AddSingleton<IBeforeExecutionHandler, UserContextBeforeHandler>();
        services.RemoveAll<IContextUpdateMatcher>();

        var sp = services.BuildServiceProvider();
        var updateProcessor = sp.GetRequiredService<IUpdateProcessor>();
        var update = new Update
        {
            Message = new Message()
            {
                From = new User()
                {
                    Id = 1,
                    LanguageCode = "ru"
                }
            }
        };

        // Act
        await updateProcessor.HandleUpdate(update, CancellationToken.None);
    }

    [Fact]
    public async Task Executes_before_and_handlers_in_the_different_async_contexts()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTelegramBotHandling(typeof(UserContextBeforeHandler).Assembly);
        services.AddSingleton<IBeforeExecutionHandler, UserContextBeforeHandler>();

        var sp = services.BuildServiceProvider();
        var updateProcessor = sp.GetRequiredService<IUpdateProcessor>();
        var update = new Update
        {
            Message = new Message()
            {
                From = new User()
                {
                    Id = 1,
                    LanguageCode = "ru"
                }
            }
        };

        // Act
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await updateProcessor.HandleUpdate(update, CancellationToken.None));
    }
}