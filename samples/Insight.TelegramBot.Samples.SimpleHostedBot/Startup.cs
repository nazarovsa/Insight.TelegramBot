using System.Net.Http;
using Insight.TelegramBot.Configurations;
using Insight.TelegramBot.Samples.Domain;
using Insight.TelegramBot.Web.Hosts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Insight.TelegramBot.Samples.SimpleHostedBot
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<BotConfiguration>(Configuration.GetSection(nameof(BotConfiguration)));

            services.AddSingleton<IHostedBot, SampleHostedBot>(c => new SampleHostedBot(
                c.GetService<IOptions<BotConfiguration>>().Value, c.GetService<ITelegramBotClient>()));

            services.AddSingleton<ITelegramBotClient, TelegramBotClient>(c =>
                new TelegramBotClient(c.GetService<IOptions<BotConfiguration>>().Value.Token,
                    new HttpClient()));

            services.AddHostedService<TelegramBotWebHost>();
        }
    }
}