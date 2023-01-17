using Insight.TelegramBot.Handling.Matchers.TextMatchers;

namespace Insight.TelegramBot.Samples.Handling.StartMessage;

public sealed class StartMessageMatcher : TextEqualsUpdateMatcher
{
    public StartMessageMatcher()
    {
        Template = "/start";
    }
}