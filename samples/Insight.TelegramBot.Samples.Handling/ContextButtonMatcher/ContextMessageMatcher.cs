using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Handling.Matchers;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Samples.Handling.ContextButtonMatcher;

public sealed class ContextMessageMatcher : IContextUpdateMatcher
{
    private readonly IDummy _scopedService;

    public ContextMessageMatcher(IDummy scopedService)
    {
        _scopedService = scopedService;
    }

    public Task<bool> MatchesAsync(Update update, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_scopedService.IsTrue());
    }
}