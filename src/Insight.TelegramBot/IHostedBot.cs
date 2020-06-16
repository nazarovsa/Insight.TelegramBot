using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Insight.TelegramBot
{
    public interface IHostedBot
    {
        Task Start();

        Task Stop();

        Task ProcessMessage(Message message, CancellationToken token = default);

        Task ProcessCallback(CallbackQuery query, CancellationToken token = default);
    }
}