using Insight.TelegramBot.Handling.Matchers;

namespace Insight.TelegramBot.Handling.Handlers;

public interface IMatchingUpdateHandler<TMatcher> : IUpdateHandler
    where TMatcher : IUpdateMatcher, new()
{
    static TMatcher Matcher { get; }
}