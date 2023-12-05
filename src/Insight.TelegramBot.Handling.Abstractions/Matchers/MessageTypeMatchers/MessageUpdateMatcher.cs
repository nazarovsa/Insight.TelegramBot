using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Insight.TelegramBot.Handling.Matchers.MessageTypeMatchers;

/// <summary>
/// Abstract update matcher for messages.
/// </summary>
public abstract class MessageUpdateMatcher : IUpdateMatcher
{
    /// <summary>
    /// Is <see cref="UpdateType"/> a message.
    /// </summary>
    /// <param name="update"><see cref="Update"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    public virtual Task<bool> MatchesAsync(Update update, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(update.Type == UpdateType.Message);
    }
}