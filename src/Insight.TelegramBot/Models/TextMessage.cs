namespace Insight.TelegramBot.Models
{
    public class TextMessage : BotMessage
    {
        public string Text { get; set; }

        public bool DisableWebPagePreview { get; set; } = false;
    }
}