using Telegram.Bot.Types;

namespace Insight.TelegramBot.Handling.Matchers;

public interface IUpdateMatcher
{
    public bool Matches(Update update);
}