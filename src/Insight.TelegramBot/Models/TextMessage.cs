using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models;

public class TextMessage : BotMessage
{
    public TextMessage(ChatId chatId) : base(chatId)
    {
    }

    public string Text { get; set; }
}