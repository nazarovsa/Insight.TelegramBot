using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models;

public class VoiceMessage : BotMessageWithFile
{
    public VoiceMessage(ChatId chatId, InputFile inputFile) : base(chatId, inputFile)
    {
    }

    public int Duration { get; set; }
}