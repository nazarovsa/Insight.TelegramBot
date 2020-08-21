using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public class VideoNoteMessage : BotMessageWithFile
    {
        public VideoNoteMessage(ChatId chatId) : base(chatId)
        {
        }

        public int Duration { get; set; }
        
        public int Length { get; set; }
    }
}