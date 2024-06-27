using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public class VideoNoteMessage : BotMessageWithFile
    {
        public VideoNoteMessage(ChatId chatId, InputFile inputFile) : base(chatId, inputFile)
        {
        }

        public int Duration { get; set; }
        
        public int Length { get; set; }
    }
}