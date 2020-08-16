using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public sealed class AnimationMessage : BotMessageWithFile
    {
        public AnimationMessage(ChatId chatId) : base(chatId)
        {
        }

        public int Duration { get; set; } = 0;

        public int Width { get; set; } = 0;

        public int Height { get; set; } = 0;

        public InputMedia Thumb { get; set; } = null;
    }
}