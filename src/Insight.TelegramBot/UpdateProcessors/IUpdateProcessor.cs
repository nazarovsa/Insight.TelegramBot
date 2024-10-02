using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.UpdateProcessors;

public interface IUpdateProcessor
{
    Task HandleUpdate(Update update, CancellationToken cancellationToken = default);
}