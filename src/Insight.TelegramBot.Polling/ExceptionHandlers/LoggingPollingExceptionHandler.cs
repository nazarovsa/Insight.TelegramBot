using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Insight.TelegramBot.Polling.ExceptionHandlers;

public sealed class LoggingPollingExceptionHandler : IPollingExceptionHandler
{
    private readonly ILogger<LoggingPollingExceptionHandler> _logger;

    public LoggingPollingExceptionHandler(ILogger<LoggingPollingExceptionHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task HandlePollingException(Exception exception, CancellationToken cancellationToken = default)
    {
        _logger.LogError(exception, "Exception occured during polling");
        return Task.CompletedTask;
    }
}