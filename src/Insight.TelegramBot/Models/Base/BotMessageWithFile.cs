using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace Insight.TelegramBot.Models
{
    public abstract class BotMessageWithFile : BotMessage
    {
        protected BotMessageWithFile(ChatId chatId) : base(chatId)
        {
        }

        public InputOnlineFile InputOnlineFile { get; set; }

        public string Caption { get; set; } = null;
    }
}