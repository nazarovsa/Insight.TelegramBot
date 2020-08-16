using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Insight.TelegramBot.Models
{
    public class GameMessage : BotMessage
    {
        public GameMessage(ChatId chatId) : base(chatId)
        {
        }

        public string GameShortName { get; set; }

        public new InlineKeyboardMarkup ReplyMarkup { get; set; }
    }
}