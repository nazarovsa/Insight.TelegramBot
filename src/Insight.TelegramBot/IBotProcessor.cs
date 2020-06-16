using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Insight.TelegramBot
{
    public interface IBotProcessor
    {
        Task ProcessMessage(Message message);

        Task ProcessCallback(CallbackQuery callbackQuery);
    }
}