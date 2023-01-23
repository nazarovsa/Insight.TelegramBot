using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Insight.TelegramBot.Handling.Matchers.TextMatchers;

/// <summary>
/// Abstract update matcher which
/// </summary>
public abstract class TextUpdateMatcher : IUpdateMatcher
{
    /// <summary>
    /// Is <see cref="UpdateType"/> a message.
    /// </summary>
    /// <param name="update"><see cref="Update"/>.</param>
    public virtual bool Matches(Update update)
    {
        return update.Type == UpdateType.Message;
    }
}