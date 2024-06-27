using System.Reflection;
using Insight.TelegramBot.WebHook.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.TelegramBot.DependencyInjection.WebHook.Controllers;

public static class WebHookHostBuilderExtensions
{
    /// <summary>
    /// Registers <see cref="DefaultUpdateController"/> as a service
    /// </summary>
    /// <param name="builder"><see cref="IMvcBuilder"/></param>
    /// <returns></returns>
    public static WebHookHostBuilder WithDefaultUpdateController(this WebHookHostBuilder builder)
    {
        var assembly = Assembly.Load(typeof(DefaultUpdateController).Assembly.GetName());
        if (assembly == null)
            throw new ArgumentNullException(nameof(assembly));

        builder.Services.AddMvc()
            .AddApplicationPart(assembly)
            .AddControllersAsServices();

        return builder;
    }
}