using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Insight.TelegramBot
{
    public interface IBotProcessor
    {
        Task ProcessMessage(Message message, CancellationToken cancellationToken = default);

        Task ProcessCallback(CallbackQuery callbackQuery, CancellationToken cancellationToken = default);
    }
}