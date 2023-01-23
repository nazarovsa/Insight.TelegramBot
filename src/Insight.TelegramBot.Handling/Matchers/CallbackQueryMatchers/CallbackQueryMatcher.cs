using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Insight.TelegramBot.Handling.Matchers.CallbackQueryMatchers;

public abstract class CallbackQueryMatcher : IUpdateMatcher
{
    public virtual bool Matches(Update update)
    {
        return update.Type == UpdateType.CallbackQuery;
    }
}