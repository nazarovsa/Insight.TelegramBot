namespace Insight.TelegramBot.Models
{
    public class PhotoMessage : BotMessageWithFile
    {
        public string Caption { get; set; } = null;
    }
}