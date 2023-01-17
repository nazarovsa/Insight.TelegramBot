using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Handling.Handlers;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Samples.Handling.ClickButton;

public sealed class ClickButtonHandler : MatchingUpdateHandler<ClickButtonMatcher>
{
    private readonly IDummy _dummy;

    public ClickButtonHandler(IDummy dummy)
    {
        _dummy = dummy;
    }

    public override Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        _dummy.Handle();
        return Task.CompletedTask;
    }
}