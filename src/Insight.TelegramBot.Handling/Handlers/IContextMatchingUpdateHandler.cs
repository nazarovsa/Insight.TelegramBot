using Insight.TelegramBot.Handling.Matchers;

namespace Insight.TelegramBot.Handling.Handlers;

public interface IContextMatchingUpdateHandler<TMatcher> : IUpdateHandler
    where TMatcher : IContextUpdateMatcher
{
}