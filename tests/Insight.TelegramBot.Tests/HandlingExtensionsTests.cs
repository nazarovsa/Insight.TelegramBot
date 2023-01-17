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
        var typeMap = provider.GetTypeMap().ToArray();

        // Assert
        Assert.Equal(typeof(StartMessageHandler), handler.GetType());
        Assert.Equal(2, typeMap.Length);
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
        var dummy = Substitute.For<IDummy>();
        services.AddSingleton(dummy);
        
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
}