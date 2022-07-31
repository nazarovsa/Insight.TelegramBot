using System;
using System.Threading;
using System.Threading.Tasks;

namespace Insight.TelegramBot;

public interface IPollingUpdateProcessor : IUpdateProcessor
{
    Task HandlePollingErrorAsync(Exception exception, CancellationToken cancellationToken = default);
}
