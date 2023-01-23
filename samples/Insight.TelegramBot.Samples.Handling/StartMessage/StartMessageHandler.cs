using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Handling.Handlers;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Samples.Handling.StartMessage;

public sealed class StartMessageHandler : IMatchingUpdateHandler<StartMessageMatcher>
{
    public Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        Console.WriteLine(update.Message!.Text);
        return Task.CompletedTask;
    }
}