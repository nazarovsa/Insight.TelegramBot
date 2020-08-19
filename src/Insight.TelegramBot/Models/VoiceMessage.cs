using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public class VoiceMessage : BotMessageWithFile
    {
        public VoiceMessage(ChatId chatId) : base(chatId)
        {
        }

        public int Duration { get; set; }
    }
}