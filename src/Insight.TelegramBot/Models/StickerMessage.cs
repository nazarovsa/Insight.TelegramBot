using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public sealed class StickerMessage : BotMessageWithFile
    {
        public StickerMessage(ChatId chatId, InputFile inputFile) : base(chatId, inputFile)
        {
        }

        public string? Emoji { get; set; }
    }
}