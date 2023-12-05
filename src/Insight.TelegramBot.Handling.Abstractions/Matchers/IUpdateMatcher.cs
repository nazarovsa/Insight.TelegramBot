using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling.Matchers;

public interface IUpdateMatcher
{
    public Task<bool> MatchesAsync(Update update, CancellationToken cancellationToken = default);
}