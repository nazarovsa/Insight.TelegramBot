using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public abstract class BotMessageWithFile : BotMessage
    {
        protected BotMessageWithFile(ChatId chatId, InputFile inputFile) : base(chatId)
        {
            InputFile = inputFile;
        }

        public InputFile InputFile { get; }
        
        public InputFile? Thumbnail { get; set; }

        public bool HasSpoiler { get; set; }

        public string? Caption { get; set; }

        public bool ShowCaptionAboveMedia { get; set; }
    }
}