using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Insight.TelegramBot.Handling.Matchers.TextMatchers;

/// <summary>
/// Abstract update matcher for text messages.
/// </summary>
public abstract class TextUpdateMatcher : IUpdateMatcher
{
    /// <summary>
    /// Is <see cref="UpdateType"/> a message.
    /// </summary>
    /// <param name="update"><see cref="Update"/>.</param>
    public virtual Task<bool> MatchesAsync(Update update)
    {
        return Task.FromResult(update.Type == UpdateType.Message && update.Message!.Type == MessageType.Text);
    }
}