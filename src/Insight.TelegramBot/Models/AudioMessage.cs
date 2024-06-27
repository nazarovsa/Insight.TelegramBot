using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public sealed class AudioMessage : BotMessageWithFile
    {
        public AudioMessage(ChatId chatId, InputFile inputFile) : base(chatId, inputFile)
        {
        }

        public string? Performer { get; set; }

        public string? Title { get; set; }

        public int Duration { get; set; }
    }
}