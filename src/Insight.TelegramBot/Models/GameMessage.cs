using Telegram.Bot.Types.ReplyMarkups;

namespace Insight.TelegramBot.Models
{
    public class GameMessage : BotMessage
    {
        public string GameShortName { get; set; }

        public new InlineKeyboardMarkup ReplyMarkup { get; set; }
    }
}