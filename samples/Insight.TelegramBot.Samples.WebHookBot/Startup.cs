using Insight.TelegramBot.Hosting.DependencyInjection.Infrastructure;
using Insight.TelegramBot.Hosting.Options;
using Insight.TelegramBot.Samples.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace Insight.TelegramBot.Samples.WebHookBot
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
                builder.WithBot<SampleBot>()
                    .WithTelegramBotClient(client =>
                        client.WithMicrosoftHttpClientFactory()
                            .WithLifetime(ServiceLifetime.Transient))
                    .WithOptions(opt => opt.FromConfiguration(Configuration))
                    .WithUpdateProcessor<SampleUpdateProcessor>()
                    .WithWebHook(webhook => webhook.WithDefaultUpdateController()));

            services.AddMvc()
                .AddNewtonsoftJson(opt =>
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.AddUpdateControllerRoute(
                    app.ApplicationServices.GetRequiredService<TelegramBotOptions>().WebHook!.WebHookPath);
            });
        }
    }
}