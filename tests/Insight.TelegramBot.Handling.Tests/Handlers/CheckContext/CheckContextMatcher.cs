using Insight.TelegramBot.Handling.Matchers;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling.Tests.Handlers.CheckContext;

public sealed class CheckContextMatcher : IContextUpdateMatcher
{
    public Task<bool> MatchesAsync(Update update, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}