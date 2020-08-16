namespace Insight.TelegramBot.Models
{
    public class LocationMessage : BotMessage
    {
        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public int LivePeriod { get; set; }
    }
}