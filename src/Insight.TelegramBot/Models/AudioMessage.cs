using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public sealed class AudioMessage : BotMessageWithFile
    {
        public AudioMessage(ChatId chatId) : base(chatId)
        {
        }

        public string Performer { get; set; } = null;

        public string Title { get; set; } = null;

        public int Duration { get; set; } = 0;
    }
}