using System;
using System.Threading;
using System.Threading.Tasks;

namespace Insight.TelegramBot.Hosting.Polling.ExceptionHandlers;

public interface IPollingExceptionHandler
{
    Task HandlePollingException(Exception exception, CancellationToken cancellationToken = default);
}