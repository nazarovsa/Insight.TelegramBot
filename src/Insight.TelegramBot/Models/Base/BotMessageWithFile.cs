using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public abstract class BotMessageWithFile : BotMessage
    {
        protected BotMessageWithFile(ChatId chatId) : base(chatId)
        {
        }

        public InputFile InputFile { get; set; }
        
        public InputFile? Thumbnail { get; set; } = null;

        public bool HasSpoiler { get; set; }

        public string Caption { get; set; } = null;

        public bool ShowCaptionAboveMedia { get; set; }
    }
}