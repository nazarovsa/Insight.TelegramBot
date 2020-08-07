using Telegram.Bot.Types.InputFiles;

namespace Insight.TelegramBot.Models
{
    public abstract class BotMessageWithFile : BotMessage
    {
        public InputOnlineFile InputOnlineFile { get; set; }
        
        public string Caption { get; set; } = null;
    }
}