using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling.Handlers;

public interface IBeforeExecutionHandler
{
    Task Handle(Update update, CancellationToken cancellationToken = default);
}