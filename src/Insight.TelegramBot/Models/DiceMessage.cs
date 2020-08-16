using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Insight.TelegramBot.Models
{
    public class DiceMessage : BotMessage
    {
        public DiceMessage(ChatId chatId) : base(chatId)
        {
        }

        public Emoji? Emoji { get; set; }
    }
}