using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public class DiceMessage : BotMessage
    {
        public DiceMessage(ChatId chatId) : base(chatId)
        {
        }

        public string? Emoji { get; set; }
    }
}