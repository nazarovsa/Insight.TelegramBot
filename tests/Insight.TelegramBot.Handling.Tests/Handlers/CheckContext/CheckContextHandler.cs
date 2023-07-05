using Insight.TelegramBot.Handling.Handlers;
using Insight.TelegramBot.Testing;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling.Tests.Handlers.CheckContext;

public class CheckContextHandler : IContextMatchingUpdateHandler<CheckContextMatcher>
{
    public static readonly string ErrorMessage = "Empty context";

    public Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        var context = ContextProvider<UserContext>.GetContext();
        if (context == null)
        {
            throw new InvalidOperationException(ErrorMessage);
        }

        return Task.CompletedTask;
    }
}