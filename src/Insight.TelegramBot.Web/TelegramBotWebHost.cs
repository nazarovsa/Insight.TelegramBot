using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Insight.TelegramBot.Web
{
    public class TelegramBotWebHost : IHostedService
    {
        private readonly IBot _bot;

        public TelegramBotWebHost(IBot bot)
        {
            if (bot == null)
                throw new ArgumentNullException(nameof(bot));

            _bot = bot;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _bot.Start();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _bot.Stop();
        }
    }
}