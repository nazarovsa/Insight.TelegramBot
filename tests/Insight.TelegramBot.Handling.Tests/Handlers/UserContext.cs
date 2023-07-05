using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Insight.TelegramBot.Handling.Tests.Handlers;

public class UserContext
{
    public long Id { get; set; }

    public string Culture { get; set; }

    public static UserContext FromUpdate(Update update)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                return new UserContext
                {
                    Id = update.Message.From.Id,
                    Culture = update.Message.From.LanguageCode
                };
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}