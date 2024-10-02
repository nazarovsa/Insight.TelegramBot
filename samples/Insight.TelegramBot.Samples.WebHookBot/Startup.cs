using Insight.TelegramBot.DependencyInjection.Infrastructure;
using Insight.TelegramBot.DependencyInjection.WebHook;
using Insight.TelegramBot.DependencyInjection.WebHook.Controllers;
using Insight.TelegramBot.Samples.Domain;
using Insight.TelegramBot.WebHook;
using Insight.TelegramBot.WebHook.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.TelegramBot.Samples.WebHookBot;

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
            builder.WithTelegramBotClient(client =>
                    client.WithMicrosoftHttpClientFactory()
                        .WithLifetime(ServiceLifetime.Transient))
                .WithOptions(opt => opt.FromConfiguration(Configuration))
                .WithUpdateProcessor<SampleUpdateProcessor>()
                .WithWebHook(webhook => webhook.WithDefaultUpdateController()));

        services.AddMvc();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.AddUpdateControllerRoute(
                app.ApplicationServices.GetRequiredService<WebHookOptions>()!.WebHookPath);
        });
    }
}