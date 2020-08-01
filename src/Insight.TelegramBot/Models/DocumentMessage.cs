namespace Insight.TelegramBot.Models
{
    public class DocumentMessage : BotMessageWithFile
    {
        public string Caption { get; set; } = null;
    }
}