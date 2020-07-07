using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Models;
using Telegram.Bot.Types;

namespace Insight.TelegramBot
{
    public interface IBot
    {
        Task<Message> SendMessage(BotMessage message, CancellationToken cancellationToken = default);

        Task DeleteMessage(long chatId, int messageId, CancellationToken cancellationToken = default);
    }
}