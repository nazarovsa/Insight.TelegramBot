using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Insight.TelegramBot
{
    public interface IUpdateProcessor
    {
        Task ProcessUpdate(Update update, CancellationToken cancellationToken = default);
    }
}