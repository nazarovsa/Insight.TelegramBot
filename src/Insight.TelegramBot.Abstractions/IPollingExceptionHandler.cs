using System;
using System.Threading;
using System.Threading.Tasks;

namespace Insight.TelegramBot.Abstractions;

public interface IPollingExceptionHandler
{
    Task HandlePollingErrorAsync(Exception exception, CancellationToken cancellationToken = default);
}
