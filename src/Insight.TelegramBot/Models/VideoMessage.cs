using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public class VideoMessage : BotMessageWithFile
    {
        public VideoMessage(ChatId chatId, InputFile inputFile) : base(chatId, inputFile)
        {
        }
        
        public int Duration { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool SupportsStreaming { get; set; }
    }
}