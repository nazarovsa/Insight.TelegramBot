using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Handling.Matchers;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling.Handlers;

public abstract class MatchingUpdateHandler<TMatcher> : IMatchingUpdateHandler<TMatcher>
    where TMatcher : IUpdateMatcher, new()
{
    public static TMatcher Matcher => _matcher;

    private static readonly TMatcher _matcher = new();
    
    public abstract Task Handle(Update update, CancellationToken cancellationToken = default);
}