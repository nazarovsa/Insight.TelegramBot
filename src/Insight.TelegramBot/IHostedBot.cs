using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Insight.TelegramBot
{
    public interface IHostedBot
    {
        Task Start();

        Task Stop();

        Task ProcessMessage(Message message, CancellationToken cancellationToken = default);

        Task ProcessCallback(CallbackQuery callbackQuery, CancellationToken cancellationToken = default);
    }
}