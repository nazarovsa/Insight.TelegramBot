using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models;

public class DocumentMessage : BotMessageWithFile
{
    public DocumentMessage(ChatId chatId, InputFile inputFile) : base(chatId, inputFile)
    {
    }

    public bool DisableContentTypeDetection { get; set; }
}