using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Configurations;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Testing
{
    public sealed class TestHostedBot : HostedBot
    {
        public TestHostedBot(BotConfiguration config, ITelegramBotClient client) : base(config, client)
        {
        }

        public override Task ProcessMessage(Message message, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override Task ProcessCallback(CallbackQuery callbackQuery, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}