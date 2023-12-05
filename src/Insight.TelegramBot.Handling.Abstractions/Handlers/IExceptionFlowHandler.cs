using System;
using System.Threading;
using System.Threading.Tasks;

namespace Insight.TelegramBot.Handling.Handlers;

public interface IExceptionFlowHandler
{
    Task Handle(Exception ex, CancellationToken cancellationToken = default);
}