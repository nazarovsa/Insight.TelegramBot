using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public class LocationMessage : BotMessage
    {
        public LocationMessage(ChatId chatId) : base(chatId)
        {
        }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public int LivePeriod { get; set; }
    }
}