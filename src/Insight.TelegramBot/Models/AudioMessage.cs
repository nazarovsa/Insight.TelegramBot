namespace Insight.TelegramBot.Models
{
    public sealed class AudioMessage : BotMessageWithFile
    {
        public string Performer { get; set; } = null;
        
        public string Title { get; set; } = null;
        
        public int Duration { get; set; } = 0;
    }
}