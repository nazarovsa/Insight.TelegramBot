using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling.Matchers.CallbackQueryMatchers;

public class StateCallbackQueryMatcher<TState> : CallbackQueryMatcher
    where TState : Enum
{
    public TState ExpectingState { get; protected set; }

    public override async Task<bool> MatchesAsync(Update update)
    {
        if (!await base.MatchesAsync(update))
            return false;

        var callbackData = CallbackData<TState>.Parse(update.CallbackQuery!.Data);
        
        return ExpectingState.Equals(callbackData.NextState);
    }
}