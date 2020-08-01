namespace Insight.TelegramBot.Models
{
    public sealed class AudioMessage : BotMessageWithFile
    {
        public string Caption { get; set; } = null;
        
        public string Performer { get; set; } = null;
        
        public string Title { get; set; } = null;
        
        public int Duration { get; set; } = 0;
    }
}