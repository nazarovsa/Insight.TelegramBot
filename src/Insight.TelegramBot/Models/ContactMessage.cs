using Telegram.Bot.Types;

namespace Insight.TelegramBot.Models;

public class ContactMessage : BotMessage
{
    public ContactMessage(ChatId chatId) : base(chatId)
    {
    }

    public string PhoneNumber { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
        
    public string VCard { get; set; }
}