using Insight.TelegramBot.Handling.Matchers.CallbackQueryMatchers;

namespace Insight.TelegramBot.Samples.Handling.ClickButton;

public sealed class ClickButtonMatcher : StateCallbackQueryMatcher<HandlingState>
{
    public ClickButtonMatcher()
    {
        ExpectingState = HandlingState.New;
    }
}