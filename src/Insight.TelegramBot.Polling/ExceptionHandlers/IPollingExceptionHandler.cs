using System;
using System.Threading;
using System.Threading.Tasks;

namespace Insight.TelegramBot.Polling.ExceptionHandlers;

public interface IPollingExceptionHandler
{
    Task HandlePollingException(Exception exception, CancellationToken cancellationToken = default);
}