using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public sealed class DocumentMessage : BotMessageWithFile
    {
        public DocumentMessage(ChatId chatId) : base(chatId)
        {
        }
    }
}