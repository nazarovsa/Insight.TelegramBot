using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public sealed class StickerMessage : BotMessageWithFile
    {
        public StickerMessage(ChatId chatId) : base(chatId)
        {
        }
    }
}