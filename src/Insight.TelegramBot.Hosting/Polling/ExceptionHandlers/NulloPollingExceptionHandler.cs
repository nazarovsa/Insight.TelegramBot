using System;
using System.Threading;
using System.Threading.Tasks;

namespace Insight.TelegramBot.Hosting.Polling.ExceptionHandlers;

public sealed class NulloPollingExceptionHandler : IPollingExceptionHandler
{
    public Task HandlePollingException(Exception exception, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}