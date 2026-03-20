using Insight.TelegramBot.DependencyInjection;
using Insight.TelegramBot.DependencyInjection.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Xunit;

namespace Insight.TelegramBot.Tests;

public class TelegramBotClientBuilderTests
{
    // Token format: {botId}:{secret} — no real network calls are made during construction
    private const string TestToken = "1234567890:AAHdqTcvCH1vGWJxfSeofSAs0K5PALDsaw";

    [Fact]
    public void AddTelegramBot_WithoutBaseUrl_RegistersClientWithDefaultApiServer()
    {
        var services = new ServiceCollection();
        services.AddTelegramBot(b =>
        {
            b.WithOptions(o => o.FromValue(new TelegramBotOptions { Token = TestToken }));
            b.WithTelegramBotClient(_ => { });
        });

        var client = services.BuildServiceProvider().GetRequiredService<ITelegramBotClient>();

        Assert.False(client.LocalBotServer);
    }

    [Fact]
    public void AddTelegramBot_WithBaseUrl_RegistersClientWithCustomApiServer()
    {
        const string customBaseUrl = "http://localhost:8081";

        var services = new ServiceCollection();
        services.AddTelegramBot(b =>
        {
            b.WithOptions(o => o.FromValue(new TelegramBotOptions
            {
                Token = TestToken,
                BaseUrl = customBaseUrl
            }));
            b.WithTelegramBotClient(_ => { });
        });

        var client = services.BuildServiceProvider().GetRequiredService<ITelegramBotClient>();

        Assert.True(client.LocalBotServer);
    }
}
