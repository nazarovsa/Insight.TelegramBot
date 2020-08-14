using Telegram.Bot.Types.Enums;

namespace Insight.TelegramBot.Models
{
    public class DiceMessage : BotMessage
    {
        public Emoji? Emoji { get; set; }
    }
}