using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Insight.TelegramBot.Web.Hosts
{
    public class TelegramBotWebHost : IHostedService
    {
        private readonly IHostedBot _bot;

        public TelegramBotWebHost(IHostedBot bot)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _bot.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _bot.Stop();
            return Task.CompletedTask;
        }
    }
}