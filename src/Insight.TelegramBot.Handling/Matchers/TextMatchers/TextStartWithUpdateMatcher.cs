using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling.Matchers.TextMatchers;

public abstract class TextStartWithUpdateMatcher : TextUpdateMatcher
{
    public string Template { get; protected set; } = null!;

    public StringComparison StringComparison { get; protected set; } = StringComparison.OrdinalIgnoreCase;

    public override async Task<bool> MatchesAsync(Update update, CancellationToken cancellationToken = default)
    {
        if (!await base.MatchesAsync(update, cancellationToken))
            return false;

        return update.Message!.Text!.StartsWith(Template, StringComparison);
    }
}
