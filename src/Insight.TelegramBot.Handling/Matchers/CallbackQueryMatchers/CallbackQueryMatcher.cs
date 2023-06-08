using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Insight.TelegramBot.Handling.Matchers.CallbackQueryMatchers;

public abstract class CallbackQueryMatcher : IUpdateMatcher
{
    public virtual Task<bool> MatchesAsync(Update update)
    {
        return Task.FromResult(update.Type == UpdateType.CallbackQuery);
    }
}