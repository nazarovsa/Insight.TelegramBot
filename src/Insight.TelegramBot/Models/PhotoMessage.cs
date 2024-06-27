using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public class PhotoMessage : BotMessageWithFile
    {
        public PhotoMessage(ChatId chatId, InputFile inputFile) : base(chatId, inputFile)
        {
        }
    }
}