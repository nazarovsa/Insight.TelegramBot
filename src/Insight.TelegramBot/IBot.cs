using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Models;
using Telegram.Bot.Types;

namespace Insight.TelegramBot
{
    public interface IBot
    {
        Task Start();

        Task Stop();

        Task<Message> SendMessage(BotMessage message, CancellationToken token = default);

        Task DeleteMessage(long chatId, int messageId, CancellationToken token = default);

        Task ProcessMessage(Message message, CancellationToken token = default);

        Task ProcessCallback(CallbackQuery query, CancellationToken token = default);
    }
}