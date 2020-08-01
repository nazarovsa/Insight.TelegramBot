using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Insight.TelegramBot.Models
{
    public abstract class BotMessage
    {
        public ChatId ChatId { get; set; }

        public ParseMode ParseMode { get; set; } = ParseMode.Default;

        public bool DisableNotification { get; set; } = false;

        public int ReplyToMessageId { get; set; } = 0;

        public IReplyMarkup ReplyMarkup { get; set; } = null;
    }
}