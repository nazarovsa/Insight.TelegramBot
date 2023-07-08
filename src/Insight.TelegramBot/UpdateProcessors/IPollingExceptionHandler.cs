using System;
using System.Threading;
using System.Threading.Tasks;

namespace Insight.TelegramBot.UpdateProcessors;

public interface IPollingExceptionHandler
{
    Task HandlePollingErrorAsync(Exception exception, CancellationToken cancellationToken = default);
}
