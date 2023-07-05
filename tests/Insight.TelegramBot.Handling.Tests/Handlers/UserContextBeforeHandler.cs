using Insight.TelegramBot.Handling.Handlers;
using Insight.TelegramBot.Testing;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling.Tests.Handlers;

public class UserContextBeforeHandler : IBeforeExecutionHandler
{
    public Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        var context = UserContext.FromUpdate(update);
        ContextProvider<UserContext>.SetContext(context);
        return Task.CompletedTask;
    }
}