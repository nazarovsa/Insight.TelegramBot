using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Handling.Matchers.MessageTypeMatchers;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling.Matchers.CallbackQueryMatchers;

public class ActionCallbackQueryMatcher<TState, TAction> : CallbackQueryMatcher
    where TState : struct, Enum
    where TAction : struct, Enum
{
    public TState Action { get; protected set; }

    public override async Task<bool> MatchesAsync(Update update, CancellationToken cancellationToken = default)
    {
        if (!await base.MatchesAsync(update, cancellationToken))
            return false;

        var callbackData = CallbackData<TState, TAction>.Parse(update.CallbackQuery!.Data);

        return Action.Equals(callbackData.Action);
    }
}