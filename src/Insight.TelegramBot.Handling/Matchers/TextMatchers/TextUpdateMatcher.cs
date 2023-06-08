using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Handling.Matchers.MessageTypeMatchers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Insight.TelegramBot.Handling.Matchers.TextMatchers;

/// <summary>
/// Abstract update matcher for text messages.
/// </summary>
public abstract class TextUpdateMatcher : MessageUpdateMatcher
{
    /// <summary>
    /// Is <see cref="UpdateType"/> a text message.
    /// </summary>
    /// <param name="update"><see cref="Update"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    public override async Task<bool> MatchesAsync(Update update, CancellationToken cancellationToken = default)
    {
        if (!await base.MatchesAsync(update, cancellationToken))
            return false;
        
        return update.Message!.Type == MessageType.Text;
    }
}