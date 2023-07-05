using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Handling;
using Insight.TelegramBot.Handling.Handlers;
using Insight.TelegramBot.Handling.Infrastructure;
using Insight.TelegramBot.Samples.Handling;
using Insight.TelegramBot.Samples.Handling.ClickButton;
using Insight.TelegramBot.Samples.Handling.StartMessage;
using Insight.TelegramBot.UpdateProcessors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Telegram.Bot.Types;
using Xunit;

namespace Insight.TelegramBot.Tests;

public class HandlingExtensionsTests
{
    [Fact]
    public void AddTelegramBotHandling_RegistersStartMessageHandlerAndCorrectTypeMap()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTelegramBotHandling(typeof(StartMessageHandler).Assembly);

        var sp = services.BuildServiceProvider();

        // Act
        var handler = sp.GetRequiredService<IMatchingUpdateHandler<StartMessageMatcher>>();
        var provider = sp.GetRequiredService<IUpdateHandlersProvider>();
        var typeMap = provider.TypeMap;

        // Assert
        Assert.Equal(typeof(StartMessageHandler), handler.GetType());
        Assert.Equal(2, typeMap.Count);
        var typeMapItem = typeMap.First(x => x.Key == typeof(IMatchingUpdateHandler<StartMessageMatcher>));
        Assert.Equal(typeof(IMatchingUpdateHandler<StartMessageMatcher>), typeMapItem.Key);
        Assert.Equal(typeof(StartMessageMatcher), typeMapItem.Value.GetType());
    }

    [Fact]
    public async Task AddTelegramBotHandling_RegistersClickButtonHandler_UpdateProcessor_Handle_Success()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTelegramBotHandling(typeof(StartMessageHandler).Assembly);
        services.AddOptions();
        
        var dummy = Substitute.For<IDummy>();
        services.AddSingleton(dummy);

        var logger = NullLogger<HandlingUpdateProcessor>.Instance;
        services.AddSingleton<ILogger<HandlingUpdateProcessor>>(logger);

        var sp = services.BuildServiceProvider();

        // Act
        var handler = sp.GetRequiredService<IMatchingUpdateHandler<ClickButtonMatcher>>();
        var processor = sp.GetRequiredService<IUpdateProcessor>();

        var command = new CallbackData<HandlingState>(HandlingState.New);
        var update = new Update()
        {
            CallbackQuery = new CallbackQuery
            {
                Data = command.ToString()
            }
        };

        await processor.HandleUpdate(update, CancellationToken.None);

        // Assert
        Assert.Equal(typeof(ClickButtonHandler), handler.GetType());

        dummy.Received(1).Handle();
    }
    
    [Fact]
    public async Task Add_handling_registers_handler_with_context_matcher_and_it_calls_to_scoped_service()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTelegramBotHandling(typeof(StartMessageHandler).Assembly);
        services.AddOptions();
        
        var dummy = Substitute.For<IDummy>();
        dummy.IsTrue().ReturnsForAnyArgs(true);
        services.AddSingleton(dummy);

        var logger = NullLogger<HandlingUpdateProcessor>.Instance;
        services.AddSingleton<ILogger<HandlingUpdateProcessor>>(logger);

        var sp = services.BuildServiceProvider();

        // Act
        var processor = sp.GetRequiredService<IUpdateProcessor>();

        var update = new Update()
        {
            Message = new Message() {Text = "Hi!"}
        };

        await processor.HandleUpdate(update, CancellationToken.None);

        // Assert
        dummy.Received(1).IsTrue();
    }
}