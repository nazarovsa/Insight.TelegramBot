using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Abstractions;
using Insight.TelegramBot.Handling.Matchers.MessageTypeMatchers;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling.Matchers.CallbackQueryMatchers;

public class StateCallbackQueryMatcher<TState> : CallbackQueryMatcher
    where TState : Enum
{
    public TState ExpectingState { get; protected set; }

    public override async Task<bool> MatchesAsync(Update update, CancellationToken cancellationToken = default)
    {
        if (!await base.MatchesAsync(update, cancellationToken))
            return false;

        var callbackData = CallbackData<TState>.Parse(update.CallbackQuery!.Data);
        
        return ExpectingState.Equals(callbackData.NextState);
    }
}