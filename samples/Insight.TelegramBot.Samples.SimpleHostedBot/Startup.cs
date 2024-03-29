using Insight.TelegramBot.Hosting.DependencyInjection.Infrastructure;
using Insight.TelegramBot.Hosting.Polling.ExceptionHandlers;
using Insight.TelegramBot.Samples.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddTelegramBot(builder =>
                builder.WithBot<SampleBot>(ServiceLifetime.Transient)
                    .WithTelegramBotClient(client => client.WithLifetime(ServiceLifetime.Singleton))
                    .WithOptions(opt => opt.FromConfiguration(Configuration))
                    .WithUpdateProcessor<SampleUpdateProcessor>()
                    .WithPolling(polling => polling.WithExceptionHandler<LoggingPollingExceptionHandler>()));
        }

        public void Configure(IApplicationBuilder configure)
        {
        }
    }
}