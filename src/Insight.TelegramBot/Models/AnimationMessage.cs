using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models;

public sealed class AnimationMessage : BotMessageWithFile
{
    public AnimationMessage(ChatId chatId, InputFile inputFile) : base(chatId, inputFile)
    {
    }

    public int Duration { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }
}