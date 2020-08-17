using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public class DocumentMessage : BotMessageWithFile
    {
        public DocumentMessage(ChatId chatId) : base(chatId)
        {
        }
    }
}