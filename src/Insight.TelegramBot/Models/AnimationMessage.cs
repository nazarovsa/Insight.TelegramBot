using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models
{
    public sealed class AnimationMessage : BotMessageWithFile
    {
        public int Duration { get; set; } = 0;
        
        public int Width { get; set; } = 0;
        
        public int Height { get; set; } = 0;
        
        public InputMedia Thumb { get; set; } = null;
    }
}