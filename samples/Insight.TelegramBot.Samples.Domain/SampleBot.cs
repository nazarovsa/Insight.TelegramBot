using Telegram.Bot;

namespace Insight.TelegramBot.Samples.Domain;

public sealed class SampleBot : Bot
{
    public SampleBot(ITelegramBotClient client) : base(client)
    {
    }
}
