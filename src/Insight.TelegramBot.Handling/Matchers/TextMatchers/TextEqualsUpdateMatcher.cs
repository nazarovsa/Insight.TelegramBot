using System;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling.Matchers.TextMatchers;

public abstract class TextEqualsUpdateMatcher : TextUpdateMatcher
{
    public string Template { get; protected set; } = null!;

    public StringComparison StringComparison { get; protected set; } = StringComparison.InvariantCultureIgnoreCase;

    public override bool Matches(Update update)
    {
        if (!base.Matches(update))
            return false;

        return update.Message!.Text!.Equals(Template, StringComparison);
    }
}